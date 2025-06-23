#!/usr/bin/env bash

# Usage: ./build_scripts.sh /path/to/project/

projectDirWithTrailing="$1"
librarySourceDir="${projectDirWithTrailing}Scripts/edux-scripts"
npmBuildOutDir="${projectDirWithTrailing}wwwroot/dist/js"

# Check if output dir exists and is empty
if [ ! -d "$npmBuildOutDir" ] || [ -z "$(ls -A "$npmBuildOutDir" 2>/dev/null)" ]; then
  echo "NpmBuildOutDir is empty. Running npm install..."
  cd "$librarySourceDir" || exit 1
  npm install
  echo "Building the library..."
  npm run build
else
  # Find latest source file excluding node_modules and typings.ts
  dateA=$(find "$librarySourceDir" -type f \
           ! -path "*/node_modules/*" \
           ! -name "typings.ts" \
           -printf "%T@\n" | sort -nr | head -n 1)

  # Find latest file in output dir
  dateB=$(find "$npmBuildOutDir" -type f -printf "%T@\n" | sort -nr | head -n 1)

  # Compare modification timestamps
  awk -v a="$dateA" -v b="$dateB" 'BEGIN { exit (a <= b) }'
  if [ $? -eq 0 ]; then
    echo "Library source files are newer than dist. Building the library..."
    cd "$librarySourceDir" || exit 1
    npm run build
  fi
fi
