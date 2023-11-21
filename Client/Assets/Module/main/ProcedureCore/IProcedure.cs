using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static Ninth.GameEntry.ProcedureCore;

namespace Ninth
{
    public partial class GameEntry
    {
        public interface IProcedure
        {
            Task<ProcedureInfo> Execute();
        }
    }
}

