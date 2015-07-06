extern "C"
{
    void _EnableLocalNotificationIOS8()
    {
        UIApplication *app = [UIApplication sharedApplication];
        if ([app respondsToSelector:@selector(registerUserNotificationSettings:)])
        {
            UIUserNotificationSettings *settings = [UIUserNotificationSettings settingsForTypes:UIUserNotificationTypeAlert | UIUserNotificationTypeBadge | UIUserNotificationTypeSound categories:nil];
            [app registerUserNotificationSettings:settings];
            [app registerForRemoteNotifications];
        }
    }
}