#!/bin/bash
set -euo pipefail
project_dir="$(cd "$(dirname "$0")" && pwd)"
unity_path="${CLOUDORA_UNITY_PATH:-/Applications/Unity/Hub/Editor/6000.5.9f1/Unity.app/Contents/MacOS/Unity}"
mode="${1:-build}"
[[ -x "$unity_path" ]] || { echo "Unity executable missing: $unity_path" >&2; exit 1; }
mkdir -p "$project_dir/Logs"
args=(-batchmode -nographics -projectPath "$project_dir")
case "$mode" in
  build) args+=(-quit -buildTarget iOS -executeMethod Cloudora.Editor.CloudoraIosBuildTools.BuildDevelopmentXcodeProject); marker='CLOUDORA_IOS_BUILD result=Succeeded' ;;
  configure) args+=(-quit -buildTarget iOS -executeMethod Cloudora.Editor.CloudoraIosBuildTools.Configure); marker='' ;;
  test) args+=(-buildTarget iOS -runTests -testPlatform EditMode -testResults "$project_dir/Logs/ios-editmode.xml"); marker='' ;;
  validate) args+=(-buildTarget iOS -executeMethod Cloudora.Editor.ContentValidationTool.RunBatch); marker='CLOUDORA_VALIDATION total=10000 failures=0' ;;
  *) echo "Usage: $0 [build|configure|test|validate]" >&2; exit 2 ;;
esac
log="$project_dir/Logs/ios-$mode.log"
if [[ "$mode" == test && -e "$project_dir/Logs/ios-editmode.xml" ]]; then
  mv "$project_dir/Logs/ios-editmode.xml" "$project_dir/Logs/ios-editmode-$(date +%Y%m%d%H%M%S).xml"
fi
export DEVELOPER_DIR="${DEVELOPER_DIR:-/Applications/Xcode.app/Contents/Developer}"
"$unity_path" "${args[@]}" -logFile "$log"
if [[ -n "$marker" ]]; then
  /usr/bin/grep -Fq "$marker" "$log" || { echo "Success marker missing: $log" >&2; exit 1; }
fi
if [[ "$mode" == test ]]; then
  /usr/bin/python3 - "$project_dir/Logs/ios-editmode.xml" <<'CHECK'
import sys, xml.etree.ElementTree as ET
root = ET.parse(sys.argv[1]).getroot()
assert int(root.get('passed', '0')) > 0 and int(root.get('failed', '0')) == 0 and int(root.get('inconclusive', '0')) == 0, root.attrib
assert int(root.get('total', '0')) == int(root.get('passed', '0')) + int(root.get('skipped', '0')), root.attrib
print(root.attrib)
CHECK
fi
echo "Completed $mode. Log: $log"
