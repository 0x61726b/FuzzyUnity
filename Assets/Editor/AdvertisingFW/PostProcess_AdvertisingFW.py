## Gram Post Processor
## Author: Remzi Senel (remzi@gram.gs)

import os
from sys import argv
from mod_pbxproj import XcodeProject

###########################
### CONFIGURATION START ###
###########################
# Info.plist additions.
# ( K, T, V )
# Becomes:
# <key>K</key>
# <T>V</T>
keyValuePairsToAdd = [ ('AppLovinSdkKey','string','lLrMnTcSCRJOsNhPFTNi4UKeTvtkxs7gBz2zWePi5i6x5pLKLk2PCM3gFxOBnWtzTvw-TKhd2pZ7bvSydUMqsP') ]

# System frameworks to add to xcode project.
systemFrameworksToAdd = ['StoreKit','Security','CoreTelephony','CoreGraphics','CoreData','AudioToolbox','MessageUI','QuartzCore','EventKit','EventKitUI','AVFoundation','CoreMedia','MediaPlayer','SystemConfiguration','UIKit']

# System frameworks to add to xcode project with a WEAK flag.
systemFrameworksToAddWeak = ['AdSupport']

# Custom frameworks (located in Frameworks folder of the Unity plugin under Editor) to add to project.
customFrameworksToAdd = ['GoogleMobileAds', 'FBAudienceNetwork']

# Compiler flags to add to the project
compilerFlagsToAdd = ['-fmodules']

# Linker flags to add to the project
linkerFlagsToAdd = ['-all_load','-ObjC']

# Files that require a "-fobjc-arc" flag to work.
filesThatRequireArc = [ 'AppLovinCustomEventInter.m', 'ChartboostiOSAd.m']
###########################
###  CONFIGURATION END  ###
###########################

## DO NOT EDIT BELOW THIS LINE ##

class front_appender:
    def __init__(self, fname, mode='w'):
        self.__f = open(fname, mode)
        self.__write_queue = []

    def write(self, s):
        self.__write_queue.insert(0, s)

    def close(self):
        self.__f.writelines(self.__write_queue)
        self.__f.close()

path = argv[1]
fileToAddPath = str(os.path.dirname(os.path.realpath(__file__))) + "/ObjC"
frameworkToAddPath = str(os.path.dirname(os.path.realpath(__file__))) + "/Frameworks"

print("---------- GRAM POST PROCESSOR START ----------")
print("- Path Info: ")
print('    XCode Build Path ' , path)
print('    ObjC Folder to add: ' , fileToAddPath)
print('    Framework Folder to add: ' , frameworkToAddPath)

print('- Patching Info.plist.')
f = open(path+'/Info.plist','r+')
content = f.read()
f.seek(content.find('</dict>\n</plist>'))
for pair in keyValuePairsToAdd:
    f.write('<key>' + pair[0] + '</key>\n')
    f.write('<'+pair[1]+'>' + pair[2] + '</'+pair[1]+'>')
f.write('</dict>\n</plist>')
f.close()
print('    Done.')

print('- Loading Project.')
project = XcodeProject.Load(path +'/Unity-iPhone.xcodeproj/project.pbxproj')
print('    Done.')

print('- Adding System Frameworks.')
for framework in systemFrameworksToAdd:
    project.add_file_if_doesnt_exist('System/Library/Frameworks/'+framework+'.framework', tree='SDKROOT')
print('    Done.')

print('- Adding System Frameworks (WEAK).')
for framework in systemFrameworksToAddWeak:
    project.add_file_if_doesnt_exist('System/Library/Frameworks/'+framework+'.framework', tree='SDKROOT', weak=True)
print('    Done.')

print('- Adding Custom Frameworks.')
project.add_framework_search_paths(frameworkToAddPath, recursive=False)
for framework in customFrameworksToAdd:
    project.add_file_if_doesnt_exist(frameworkToAddPath + '/'+framework+'.framework', tree='SDKROOT')
print('    Done.')

print('- Adding Objective-C Sources.')
files_in_dir = os.listdir(fileToAddPath)
for f in files_in_dir:
    if not f.startswith('.'): #ignore .DS_STORE
        pathname = os.path.join(fileToAddPath, f)
        fileName, fileExtension = os.path.splitext(pathname)
        if not fileExtension == '.meta': #ignore .meta as it is under asset server
            if os.path.isfile(pathname):
                project.add_file(pathname)
            if os.path.isdir(pathname):
                project.add_folder(pathname, excludes=["^.*\.meta$"])
print('    Done.')

print('- Adding compiler flags.')
for flag in compilerFlagsToAdd:
        project.add_other_cflags(flag)
print('    Done.')

print('- Adding linker flags.')
for flag in linkerFlagsToAdd:
    project.add_other_ldflags(flag)
print('    Done.')

print('- Adding -fobjc-arc flag for reuqired files.')
for arcFile in filesThatRequireArc:
    temp = project.get_files_by_name(arcFile)
    if temp:
        buildFiles = project.get_build_files(temp[0].id)
        if buildFiles and len(buildFiles) > 0:
            for buildFile in buildFiles:
                buildFile.add_compiler_flag('-fobjc-arc')
print('    Done.')

print('- Saving project.')
if project.modified:
  project.backup()
  project.saveFormat3_2()
print('    Done.')

print("----------  GRAM POST PROCESSOR END  ----------")
