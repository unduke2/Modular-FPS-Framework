using System.Collections.Generic;

[System.Serializable]
public class AimingData
{
    public List<AimingModeData> AimingModesData = new List<AimingModeData>();

    public AimingModeData GetAimingModeData(AimingMode aimingMode)
    {
        foreach (var data in AimingModesData)
        {
            if (data.AimingMode == aimingMode)
            {
                return data;
            }
        }
        return null;
    }
}
