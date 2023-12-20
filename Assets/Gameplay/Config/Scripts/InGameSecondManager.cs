#region

using Gameplay.Config.Scripts;

#endregion

public class InGameSecondManager : RoundManager
{
    protected override void StartRound()
    {
        base.StartRound();
        m_currentZone = ZoneType.Second;
    }
}