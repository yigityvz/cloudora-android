#import <UIKit/UIKit.h>

extern "C" int CloudoraReduceMotionEnabled()
{
    return UIAccessibilityIsReduceMotionEnabled() ? 1 : 0;
}
