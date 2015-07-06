#!/usr/bin/python

import sys
from mod_pbxproj import XcodeProject


pathToProjectFile = sys.argv[1] + '/Unity-iPhone.xcodeproj/project.pbxproj'
shouldDisableAdSupport = len( sys.argv ) > 2 and sys.argv[2] is '1'

project = XcodeProject.Load( pathToProjectFile )


if shouldDisableAdSupport:
	project.add_preprocessor_macro( 'PH_USE_AD_SUPPORT=0' )
else:
	project.add_file_if_doesnt_exist( 'System/Library/Frameworks/AdSupport.framework', tree='SDKROOT', weak=True )
	
project.add_file_if_doesnt_exist( 'System/Library/Frameworks/StoreKit.framework', tree='SDKROOT' )
if project.modified:
	project.save()