using UnityEngine;

namespace Sammy.Interfaces
{
    public interface IActivationStrategy
    {
        bool ShouldActivate(Collider activator);
    }
}