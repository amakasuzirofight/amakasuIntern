using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UniRx;
namespace Manager
{
    public interface ISetRegistBoundData
    {
        public IObserver<GameObject> SetRegistBoundData();
    }
}
