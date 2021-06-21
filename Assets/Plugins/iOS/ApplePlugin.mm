@implementation ApplePlugin

+ (float)getNativeScaleFactor
{
    UIScreen* uiScreen = [UIScreen mainScreen];
    return uiScreen.nativeScale;
}

@end

extern "C"
{
    float _GetNativeScaleFactor() 
    {
        return [ApplePlugin getNativeScaleFactor];
    }
}