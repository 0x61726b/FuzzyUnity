#import "ChartboostiOSAd.h"

@implementation ChartboostiOSAd

// Will be set by the AdMob SDK.
@synthesize delegate;

static bool _isInitialized;

#pragma mark -
#pragma mark GADCustomEventInterstitial

- (void)requestInterstitialAdWithParameter:(NSString *)serverParameter
                                     label:(NSString *)serverLabel
                                   request:(GADCustomEventRequest *)customEventRequest {
    
    if (!_isInitialized) {
        [Chartboost startWithAppId:@"542576fcc26ee42ca7e405ed"
                      appSignature:@"b6866a42f7897606dde0027153f8d636be5c57f1"
                          delegate:self];
        _isInitialized = true;
    }
    [Chartboost cacheInterstitial:CBLocationHomeScreen];
}

- (void)presentFromRootViewController:(UIViewController *)rootViewController {
    if ([Chartboost hasInterstitial:CBLocationHomeScreen]) {
        [Chartboost showInterstitial:CBLocationHomeScreen];
    }
}

//Chartboost delegates

- (void)didCacheInterstitial:(NSString *)location {
    [self.delegate customEventInterstitial:self didReceiveAd:nil];
}

- (void)didFailToLoadInterstitial:(NSString *)location withError:(CBLoadError)error {
    [self.delegate customEventInterstitial:self didFailAd:nil];
}

- (void)didDismissInterstitial:(NSString *)location {
    [self.delegate customEventInterstitialWillDismiss:self];
    [self.delegate customEventInterstitialDidDismiss:self];
}

- (BOOL)shouldRequestInterstitial:(CBLocation)location {
    return true;
}

- (BOOL)didDisplayInterstitial:(CBLocation)location {
    return true;
}

@end
