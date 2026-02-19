using UnityEngine;

// This just allows any script to call the death / damage
// function of any entity, regardless of their own script.
// Good for scalability
public interface Entity
{
    void Damage();
}
