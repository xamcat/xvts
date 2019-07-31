#!/usr/bin/env bash

BUNDLE_ID="com.xvts.superduper"
MANIFEST_FILE=SuperDuper.Droid/Properties/AndroidManifest.xml
RESOURCE_DIR=SuperDuper.Droid/Resources

target_bundle_id=
target_icon_asset=

if [ "$APPCENTER_BRANCH" == "develop" ]; then
    target_bundle_id="$BUNDLE_ID.beta"
    target_icon_asset="../tools/mipmap-beta"
elif [ "$APPCENTER_BRANCH" == "master" ]; then
    target_bundle_id="$BUNDLE_ID"
    target_icon_asset="../tools/mipmap-release"
fi

if [ -z "$target_bundle_id" ] || [ -z "$target_icon_asset" ]; then
      echo "Invalid branch"
      echo "    must be either \"master\" or \"develop\""
      exit 1
fi

# Update bundle id and version code in AndroidManifest.xml
sed -i -e "s/versionCode=\"[0-9]\{1,\}\"/versionCode=\"$APPCENTER_BUILD_ID\"/g" $MANIFEST_FILE
sed -i -e "s/package=\"\([^\"]*\)\"/package=\"$target_bundle_id\"/g" $MANIFEST_FILE

echo "Updating version.properties"
echo "    VERSION_CODE=$APPCENTER_BUILD_ID"
echo "    VERSION_BUNDLE=$target_bundle_id"

subs=`ls $RESOURCE_DIR`

for i in $subs; do
  if [[ $i == mipmap* ]]; then
    echo "Updating: $i"
    rm -rf "$RESOURCE_DIR/$i"
    cp -r "$target_icon_asset/$i" "$RESOURCE_DIR"
  fi
done