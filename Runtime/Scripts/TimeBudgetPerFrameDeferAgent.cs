﻿// Copyright 2020-2021 Andreas Atteneder
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace GLTFast {
    
    public class TimeBudgetPerFrameDeferAgent : MonoBehaviour, IDeferAgent
    {
        float lastTime;
        float timeBudget = .5f/30;

        /// <summary>
        /// Defers work to the next frame if a fix time budget is
        /// used up.
        /// </summary>
        /// <param name="frameBudget">Time budget as part of the target frame rate.</param>
        public void SetFrameBudget( float frameBudget = 0.5f )
        {
            float targetFrameRate = Application.targetFrameRate;
            if(targetFrameRate<0) targetFrameRate = 30;
            timeBudget = frameBudget/targetFrameRate;
            Reset();
        }

        void Awake() {
            SetFrameBudget();
        }

        void Update() {
            Reset();
        }

        void Reset()
        {
            lastTime = Time.realtimeSinceStartup;
        }

        public bool ShouldDefer() {
            float now = Time.realtimeSinceStartup;
            if( now-lastTime > timeBudget ) {
                lastTime = now;
                return true;
            }
            return false;
        }

        public async Task BreakPoint() {
            if (ShouldDefer()) {
                await Task.Yield();
            }
        }
    }
}