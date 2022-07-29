/*
 * Copyright 2019 Capnode AB
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); 
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using QuantConnect.Data;
using QuantConnect.Indicators;

namespace Algoloop.Algorithm.CSharp
{
    internal class MomentumSignal : ISignal
    {
        private readonly RateOfChange _roc;

        public MomentumSignal(int period)
        {
            _roc = new RateOfChange(period);
        }

        public float Update(BaseData bar, bool evaluate)
        {
            decimal close = bar.Price;
            _roc.Update(bar.Time, close);
            if (!evaluate) return 0;
            if (!_roc.IsReady) return 0;
            float roc = (float)(decimal)_roc;
            return roc;
        }

        public void Done()
        {
            throw new System.NotImplementedException();
        }
    }
}