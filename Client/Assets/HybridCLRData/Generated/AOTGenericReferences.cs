public class AOTGenericReferences : UnityEngine.MonoBehaviour
{

	// {{ constraint implement type
	// }} 

	// {{ AOT generic type
	//Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder`1<System.Byte>
	//Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder`1<System.Object>
	//Cysharp.Threading.Tasks.UniTask`1<System.Object>
	//Cysharp.Threading.Tasks.UniTask`1/Awaiter<System.Object>
	//System.Collections.Generic.Dictionary`2<System.Object,System.Object>
	//System.Collections.Generic.Dictionary`2/Enumerator<System.Object,System.Object>
	//System.Collections.Generic.KeyValuePair`2<System.Object,System.Object>
	//System.Collections.Generic.List`1<System.Object>
	//System.Func`2<System.Object,System.Object>
	// }}

	public void RefMethods()
	{
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder::AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UnityAsyncExtensions/AsyncOperationAwaiter,Ninth.HotUpdate.AssetsMgr/<UnLoadAllAsync>d__11>(Cysharp.Threading.Tasks.UnityAsyncExtensions/AsyncOperationAwaiter&,Ninth.HotUpdate.AssetsMgr/<UnLoadAllAsync>d__11&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder::Start<Ninth.HotUpdate.AssetsMgr/<UnLoadAllAsync>d__11>(Ninth.HotUpdate.AssetsMgr/<UnLoadAllAsync>d__11&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder`1<System.Byte>::AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UnityAsyncExtensions/UnityWebRequestAsyncOperationAwaiter,Ninth.HotUpdate.AssetsMgr/<Download>d__7>(Cysharp.Threading.Tasks.UnityAsyncExtensions/UnityWebRequestAsyncOperationAwaiter&,Ninth.HotUpdate.AssetsMgr/<Download>d__7&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder`1<System.Object>::AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask`1/Awaiter<System.Object>,Ninth.HotUpdate.AssetsMgr/<CloneAsync>d__8>(Cysharp.Threading.Tasks.UniTask`1/Awaiter<System.Object>&,Ninth.HotUpdate.AssetsMgr/<CloneAsync>d__8&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder`1<System.Byte>::Start<Ninth.HotUpdate.AssetsMgr/<Download>d__7>(Ninth.HotUpdate.AssetsMgr/<Download>d__7&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder`1<System.Object>::Start<Ninth.HotUpdate.AssetsMgr/<CloneAsync>d__8>(Ninth.HotUpdate.AssetsMgr/<CloneAsync>d__8&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder`1<System.Object>::Start<Ninth.HotUpdate.AssetsMgr/<LoadAssetRefAsync>d__10`1<System.Object>>(Ninth.HotUpdate.AssetsMgr/<LoadAssetRefAsync>d__10`1<System.Object>&)
		// System.Object LitJson.JsonMapper::ToObject<System.Object>(System.String)
		// System.Object System.Activator::CreateInstance<System.Object>()
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder::AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask/Awaiter,Ninth.HotUpdate.GameDriver/<Update>d__10>(Cysharp.Threading.Tasks.UniTask/Awaiter&,Ninth.HotUpdate.GameDriver/<Update>d__10&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder::AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask`1/Awaiter<System.Object>,Ninth.HotUpdate.GameDriver/<Update>d__10>(Cysharp.Threading.Tasks.UniTask`1/Awaiter<System.Object>&,Ninth.HotUpdate.GameDriver/<Update>d__10&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder::Start<Ninth.HotUpdate.GameDriver/<Awake>d__9>(Ninth.HotUpdate.GameDriver/<Awake>d__9&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder::Start<Ninth.HotUpdate.GameDriver/<Update>d__10>(Ninth.HotUpdate.GameDriver/<Update>d__10&)
		// System.Object UnityEngine.GameObject::AddComponent<System.Object>()
	}
}