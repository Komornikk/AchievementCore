# AchievementCore
Achievements system for My Summer Car mods!

Welcome fellow modder! Do you want your mods to have achievements? Well, you're in the right place.
So, to set up this mod to have your own achievements, it's quite simple.
First, download the mod itself and add it as a reference in your VisualStudio mod project:

![image](https://github.com/Komornikk/AchievementCore/assets/96838205/89446a63-8547-44b4-9d70-bdd392971964)
![image](https://github.com/Komornikk/AchievementCore/assets/96838205/203b60db-e9c0-46cf-a6ac-0ef5bc504159)

Now, add `using AchievementCore;` on the top of your Mod class:

![image](https://github.com/Komornikk/AchievementCore/assets/96838205/bef09c1b-3707-4243-b20c-5421be2c2ae3)

Adding achievements is quite easy, make sure you have the Mod_OnMenuLoad set up.

![image](https://github.com/Komornikk/AchievementCore/assets/96838205/a5afeb3e-b6b6-470a-8448-cd267f2eb4ab)

After you do that, navigate to the Mod_OnMenuLoad function, and add the following:
`if (ModLoader.IsModPresent("AchievementCore))
{
  AchievementIDHolder.achievements.Add("yourUniqueID", new AchievementIDHolder.AchievementData
            {
                mod_id = this.ID, //it can be anything (remember that this value is the name of the mod in the menu)
                name = "test achievement", //name of the achievement
                description = "very cool description", //description of the achievement
                icon = null, //your custom icon (sprite); you can null this if you want to use the default icon
                hidden = true, //leave this at true if you want it to be hidden
            });
}`
Quite easy, isn't it? Triggering achievements is also really easy! all you have to do is put this line anywhere you want to trigger the achievement!
`if (ModLoader.IsModPresent("AchievementCore")) if (ModLoader.IsModPresent("AchievementCore"));`
