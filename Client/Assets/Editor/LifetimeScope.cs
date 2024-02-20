using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Diagnostics;
using VContainer.Unity;

namespace Ninth.Editor
{
    [DefaultExecutionOrder(-5000)]
    public class LifetimeScope
    {
        public readonly struct ParentOverrideScope : IDisposable
        {
            public ParentOverrideScope(LifetimeScope nextParent)
            {
                lock (SyncRoot)
                {
                    GlobalOverrideParents.Push(nextParent);
                }
            }

            public void Dispose()
            {
                lock (SyncRoot)
                {
                    GlobalOverrideParents.Pop();
                }
            }
        }

        public readonly struct ExtraInstallationScope : IDisposable
        {
            public ExtraInstallationScope(IInstaller installer)
            {
                lock (SyncRoot)
                {
                    GlobalExtraInstallers.Push(installer);
                }
            }

            void IDisposable.Dispose()
            {
                lock (SyncRoot)
                {
                    GlobalExtraInstallers.Pop();
                }
            }
        }

        [SerializeField]
        public bool autoRun = true;

        static readonly Stack<LifetimeScope> GlobalOverrideParents = new Stack<LifetimeScope>();
        static readonly Stack<IInstaller> GlobalExtraInstallers = new Stack<IInstaller>();
        static readonly object SyncRoot = new object();


        public static ParentOverrideScope EnqueueParent(LifetimeScope parent)
            => new ParentOverrideScope(parent);

        public static ExtraInstallationScope Enqueue(Action<IContainerBuilder> installing)
            => new ExtraInstallationScope(new ActionInstaller(installing));

        public static ExtraInstallationScope Enqueue(IInstaller installer)
            => new ExtraInstallationScope(installer);

        [Obsolete("LifetimeScope.PushParent is obsolete. Use LifetimeScope.EnqueueParent instead.", false)]
        public static ParentOverrideScope PushParent(LifetimeScope parent) => new ParentOverrideScope(parent);

        [Obsolete("LifetimeScope.Push is obsolete. Use LifetimeScope.Enqueue instead.", false)]
        public static ExtraInstallationScope Push(Action<IContainerBuilder> installing) => Enqueue(installing);

        [Obsolete("LifetimeScope.Push is obsolete. Use LifetimeScope.Enqueue instead.", false)]
        public static ExtraInstallationScope Push(IInstaller installer) => Enqueue(installer);

        public IObjectResolver Container { get; private set; }
        public LifetimeScope Parent { get; private set; }
        public bool IsRoot { get; set; }

        readonly List<IInstaller> localExtraInstallers = new List<IInstaller>();

        protected virtual void Configure(IContainerBuilder builder) { }

        public IObjectResolver Build()
        {
            var builder = new ContainerBuilder
            {
                ApplicationOrigin = this,
                Diagnostics = null,
            };
            builder.RegisterBuildCallback(SetContainer);
            InstallTo(builder);
            return builder.Build();
        }

        private void SetContainer(IObjectResolver container)
        {
            Container = container;
        }

        void InstallTo(IContainerBuilder builder)
        {
            Configure(builder);

            foreach (var installer in localExtraInstallers)
            {
                installer.Install(builder);
            }
            localExtraInstallers.Clear();

            lock (SyncRoot)
            {
                foreach (var installer in GlobalExtraInstallers)
                {
                    installer.Install(builder);
                }
            }

            builder.RegisterInstance<LifetimeScope>(this).AsSelf();
            EntryPointsBuilder.EnsureDispatcherRegistered(builder);
        }
    }
}
