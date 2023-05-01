using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LeftOut.LudumDare
{
    public interface ITutorialObserver
    {
        public Action OnConditionsSatisfied { get; set; }
    }
}
