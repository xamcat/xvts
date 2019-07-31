#!/usr/bin/env bash

BUNDLE_ID="com.xvts.superduper"
APP_NAME="SuperDuper"
PROJECT_DIR="SuperDuper.iOS"
APP_ICON_SET="$PROJECT_DIR/Assets.xcassets/AppIcon.appiconset"

target_bundle_id=
target_app_name=
target_icon_asset=

if [ "$APPCENTER_BRANCH" == "develop" ]; then
    target_bundle_id="$BUNDLE_ID-beta"
    target_app_name="$APP_NAME-beta"
    target_icon_asset="tools/AppIcon-beta.appiconset"
elif [ "$APPCENTER_BRANCH" == "master" ]; then
    target_bundle_id="$BUNDLE_ID"
    target_app_name="$APP_NAME"
    target_icon_asset="tools/AppIcon-release.appiconset"
fi

if [ -z "$target_bundle_id" ] || [ -z "$target_app_name" ] || [ -z "$target_icon_asset" ]; then
      echo "Invalid branch"
      echo "    must be either \"master\" or \"develop\""
      exit 1
fi

# Update bundle name, bundle identifier, and version
plutil -replace CFBundleVersion -string $APPCENTER_BUILD_ID $PROJECT_DIR/Info.plist
plutil -replace CFBundleIdentifier -string $target_bundle_id $PROJECT_DIR/Info.plist
plutil -replace CFBundleName -string $target_app_name $PROJECT_DIR/Info.plist

# Replace app icons with our desired icons
rm -rf "$APP_ICON_SET"
cp -r "$target_icon_asset" "$APP_ICON_SET"

echo "Info.plist updated:"
echo "   CFBundleVersion=$APPCENTER_BUILD_ID"
echo "   CFBundleIdentifier=$target_bundle_id"
echo "   CFBundleName=$target_app_name"
echo ""
echo "App Icon updated:"
echo "   AppIconSets=$target_icon_asset"