using System.Collections;
using UnityEngine;
using AchievementCore;

namespace AchievementCore
{
    public class TestObject : MonoBehaviour
    {
        public void OnTriggerStay()
        {
            AchievementCore.AchievementHandler.TriggerAchievement("test_id", "Test Achievement");
        }
    }
}