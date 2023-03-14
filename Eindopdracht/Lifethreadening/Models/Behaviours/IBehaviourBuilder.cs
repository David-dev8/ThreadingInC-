using Lifethreadening.Models.Behaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    public interface IBehaviourBuilder
    {
        IBehaviourBuilder AddRest();
        IBehaviourBuilder AddEvade();
        IBehaviourBuilder AddWander(bool isNative, double traumaChance = 0.5);
        IBehaviourBuilder AddBreed(IBreedFactory breedFactory);
        IBehaviourBuilder AddEat(Diet diet);
        IBehaviourBuilder OpenComposite();
        IBehaviourBuilder CloseComposite();
        IBehaviourBuilder ForAnimal(Animal animal);
        Behaviour GetBehaviour();
    }
}
