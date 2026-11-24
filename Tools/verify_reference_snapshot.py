#!/usr/bin/env python3
"""Read-only check of the owner-approved Cloudora game baseline.

No network, build, generation, staging, save edits or manifest updates occur here.
Text line endings are normalized so Windows CRLF checkouts also compare correctly.
LFS entries are checked against the real object SHA-256, not pointer text.
"""
from __future__ import annotations
import hashlib
import json
from pathlib import Path
import subprocess
import sys

ROOT = Path(__file__).resolve().parents[1]
MANIFEST = ROOT / 'Docs' / 'REFERANS_TABAN.json'


def check(root: Path, manifest: dict) -> tuple[list[str], list[str], list[str]]:
    problems: list[str] = []
    expected = {entry['path'] for entry in manifest['files']}
    roots = tuple(manifest['protected_roots'])
    expected_single = set(manifest['protected_files'])
    for entry in manifest['files']:
        path = root / entry['path']
        if not path.is_file():
            problems.append('MISSING ' + entry['path'])
            continue
        data = path.read_bytes()
        if entry['kind'] == 'lfs' and data.startswith(b'version https://git-lfs.github.com/spec/v1\n'):
            problems.append('LFS_POINTER ' + entry['path'] + ' (run git lfs pull)')
            continue
        if entry['kind'] == 'text':
            data = data.replace(b'\r\n', b'\n')
        if hashlib.sha256(data).hexdigest() != entry['sha256']:
            problems.append('CHANGED ' + entry['path'])
    notes: list[str] = []
    extras: list[str] = []
    try:
        result = subprocess.run(
            ['git', '-C', str(root), 'ls-files', '--cached', '--others', '--exclude-standard', '-z'],
            check=True, stdout=subprocess.PIPE, stderr=subprocess.PIPE)
        paths = set(result.stdout.decode('utf-8').rstrip('\0').split('\0'))
        extras = sorted(p for p in paths if p and (p.startswith(roots) or p in expected_single) and p not in expected)
    except (OSError, subprocess.CalledProcessError):
        notes.append('Git metadata unavailable: baseline bytes were checked; new/untracked protected files could not be enumerated.')
    return problems, extras, notes


def main() -> int:
    manifest = json.loads(MANIFEST.read_text(encoding='utf-8'))
    problems, extras, notes = check(ROOT, manifest)
    print('Reference gameplay commit: ' + manifest['gameplay_commit'])
    print('Protected files checked: ' + str(len(manifest['files'])))
    for message in problems:
        print(message)
    for path in extras:
        print('ADDED ' + path)
    for note in notes:
        print('NOTE ' + note)
    if problems or extras:
        print('DIFFERENT: review whether these changes are intended and authorized. Do not discard work or rewrite the manifest just to pass.')
        return 1
    print('MATCH: protected game, asset, Editor, test, package and project-setting files match the reference.')
    return 0


if __name__ == '__main__':
    sys.exit(main())
