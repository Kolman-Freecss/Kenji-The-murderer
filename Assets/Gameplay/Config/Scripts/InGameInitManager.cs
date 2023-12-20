#region

using Gameplay.Config.Scripts;

#endregion

public class InGameInitManager : RoundManager
{
    protected override void StartRound()
    {
        base.StartRound();
        m_currentZone = ZoneType.Init;
    }
}