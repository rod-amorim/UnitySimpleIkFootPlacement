# SimpleIkFootPlacement

This is the simplest implementation of the ik foot placement algorithm.

-This implementation only works for humanoid rigs

<H1>DEMO</H1>

<img src="/SimpleIkFootDemo1.gif" width="50%" height="50%"/>
<img src="/SimpleIkFootDemo2.gif" width="50%" height="50%"/>
<img src="/SimpleIkFootDemo3.gif" width="50%" height="50%"/>

<H1>HOW TO IMPORT</H1>

<H2>Unity hierarchy</H2>
  
To this implementation work you will need: <br>
  -A imported fbx character with HUMANOID configured rig type.<br>
  -A hierarchy structure similar to this one (needs the avatar on animator component to work).<br>
  <img src="/UnityHierarchy.png" width="50%" height="100%"/><br>
  -Don't forget to enable IKPass on AnimationLayer:<br>
  <img src="/IkPass.png" width="50%" height="100%"/><br>
  
This implementation is the simplest, it should only be activated when the character is stationary.

When walking or doing any action that moves the legs, set the weight of the IKs to 0 and when the legs are stationary, set it to 1.

For that you can use the method "IsIkEnabled()" that is inside the script, whenever it returns "true" ik will be enabled and "false" will be disabled.<br>
Example:<br>
```
private bool IsIkEnabled()
{
      if (IsPlayerWalking)
          return true;
      else
          return false
}
```


  
  
