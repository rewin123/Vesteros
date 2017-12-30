using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VesterosSolver
{
    class MonteKarloSum
    {
        public List<float> datas = new List<float>();
        public void Add(float val)
        {
            datas.Add(val);
        }

        public float Medium
        {
            get
            {
                float val = 0;
                for (int i = 0; i < datas.Count; i++)
                    val += datas[i];

                return val / datas.Count;
            }
        }
    }
}
