using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVRX.Core
{
    public enum dummystates { New,Pending,Approved}

    public class TestStateFlow
    {
        public TestStateFlow()
        {
            var x = new StateFlow(dummystates.Approved);
        }
    }
    public class StateFlow
    {
        private readonly Enum _currentState;
        public StateFlow(Enum state)
        {
            _currentState = state;
        }

    }
}
