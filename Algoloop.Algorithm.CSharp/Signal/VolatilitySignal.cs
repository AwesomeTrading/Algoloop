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

using Algoloop.Algorithm.CSharp.Model;
using QuantConnect.Algorithm;
using QuantConnect.Data;
using QuantConnect.Indicators;

namespace Algoloop.Algorithm.CSharp.Signal
{
    internal class VolatilitySignal : ISignal
    {
        private readonly StandardDeviation _window;
        private readonly bool _inverse;

        public VolatilitySignal(int period, bool inverse)
        {
            if (period > 0)
            {
                _window = new StandardDeviation(period);
            }

            _inverse = inverse;
        }

        public float Update(QCAlgorithm algorithm, BaseData bar)
        {
            if (_window == null) return float.NaN;
            _window.Update(bar.EndTime, bar.Price);
            if (!_window.IsReady) return 0;

            var score = (float)_window.Current.Value * (float)Math.Sqrt(_window.Period) / (float)bar.Price;
            if (_inverse && score > 0)
            {
                score = 1 / score;
            }

            return score;
        }
    }
}
