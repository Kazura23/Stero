using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RewardOption {

	public static  void LoadReward(bool p_type, int p_numReward, RewardType p_reward)
    {
        if (p_type)
        {
            switch (p_numReward)
            {
                case 0:
                    AlwaysSuccesUnlock(p_reward);
                    break;
            }
        }
        else
        {
            switch (p_numReward)
            {
                case 0:
                    break;
            }
        }
    }

    private static void AlwaysSuccesUnlock(RewardType p_reward)
    {
        p_reward.InitReward(true, false, 0, 0);
    }

    // fonction pour chaque reward et defis
}
