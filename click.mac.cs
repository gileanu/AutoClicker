#import <Cocoa/Cocoa.h>

@interface AutoClicker : NSObject

@property BOOL autoClicking;
@property NSTimeInterval clickInterval;

- (void)startAutoClick;
- (void)stopAutoClick;

@end

@implementation AutoClicker

- (instancetype)init {
    self = [super init];
    if (self) {
        _autoClicking = NO;
        _clickInterval = 0.1; // Default click interval in seconds
    }
    return self;
}

- (void)startAutoClick {
    self.autoClicking = YES;
    dispatch_async(dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_DEFAULT, 0), ^{
        while (self.autoClicking) {
            NSPoint mouseLocation = [NSEvent mouseLocation];
            CGEventRef clickEvent = CGEventCreateMouseEvent(NULL, kCGEventLeftMouseDown, mouseLocation, kCGMouseButtonLeft);
            CGEventSetIntegerValueField(clickEvent, kCGMouseEventClickState, 1);
            CGEventPost(kCGHIDEventTap, clickEvent);
            CFRelease(clickEvent);
            [NSThread sleepForTimeInterval:self.clickInterval];
        }
    });
}

- (void)stopAutoClick {
    self.autoClicking = NO;
}

@end

int main(int argc, const char * argv[]) {
    @autoreleasepool {
        AutoClicker *autoClicker = [[AutoClicker alloc] init];
        [autoClicker startAutoClick];
        // To stop auto-clicking, call [autoClicker stopAutoClick];
        [[NSRunLoop currentRunLoop] run];
    }
    return 0;
}
