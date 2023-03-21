﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    public abstract class SimulationElement
    {
        public Location Location { get; set; }
        public int Priority { get; private set; }
        public string Image { get; set; }
        protected Action PlannedAction { get; set; }
        protected WorldContextService ContextService { get; set; }

        public SimulationElement(int priority, string image, WorldContextService contextService)
        {
            Priority = priority;
            Image = image;
            ContextService = contextService;
        }

        public void Plan()
        {
            PlannedAction = GetNextAction();
        }

        public virtual void Act()
        {
            if(PlannedAction != null)
            {
                PlannedAction();
                PlannedAction = null;
            }
        }

        protected abstract Action GetNextAction();
        public abstract bool StillExistsPhysically();
        public abstract int GetNutritionalValue();
        public abstract int DepleteNutritionalValue();
    }
}
