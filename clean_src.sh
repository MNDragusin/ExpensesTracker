#!/bin/sh

BASE_DIR="./src"

if [ ! -d "$BASE_DIR" ]; then
  echo "Directory '$BASE_DIR' not found."
  exit 1
fi

echo "Looking for 'bin' and 'obj' folders inside immediate subdirectories of '$BASE_DIR'..."

for dir in "$BASE_DIR"/*/ ; do
  [ -d "$dir" ] || continue 
  for target in bin obj; do
    target_path="$dir$target"
    if [ -d "$target_path" ]; then
      echo "Removing: $target_path"
      rm -rf "$target_path"
    fi
  done
done

echo "Done."
