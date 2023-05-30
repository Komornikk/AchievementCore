# AchievementCore
Achievements system for My Summer Car mods!

## Welcome fellow modder! Do you want your mods to have achievements? Well, you're in the right place.
## So, to set up this mod to have your own achievements, it's quite simple.
First, download the mod itself and add it as a reference in your VisualStudio mod project:

![image](https://github.com/Komornikk/AchievementCore/assets/96838205/89446a63-8547-44b4-9d70-bdd392971964)
![image](https://github.com/Komornikk/AchievementCore/assets/96838205/203b60db-e9c0-46cf-a6ac-0ef5bc504159)

Now, add `using AchievementCore;` on the top of your Mod class:

![image](https://github.com/Komornikk/AchievementCore/assets/96838205/bef09c1b-3707-4243-b20c-5421be2c2ae3)

Adding achievements is quite easy, make sure you have the **Mod_OnMenuLoad** set up.

![image](https://github.com/Komornikk/AchievementCore/assets/96838205/a5afeb3e-b6b6-470a-8448-cd267f2eb4ab)

After you do that, navigate to the **Mod_OnMenuLoad** function, and add the following:

![image](https://github.com/Komornikk/AchievementCore/assets/96838205/5e96edcc-fcda-4b3b-be52-f258275b9487)

It doesn't have to be so spaced out, I did that for tutorial purposes. Also, you can just create a function and call it **Mod_OnMenuLoad**, doesn't matter as long as it's executed in **Mod_OnMenuLoad**

Quite easy, isn't it? Triggering achievements is also really easy! all you have to do is put this line anywhere you want to trigger the achievement!

`if (ModLoader.IsModPresent("AchievementCore")) AchievementIDHolder.AchievementHandler.TriggerAchievement("mod_id", "yourUniqueID");`
