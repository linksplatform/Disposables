#!/bin/bash
set -e # Exit with nonzero exit code if anything fails

echo """
\\documentclass[11pt,a4paper,fleqn]{report}
\\usepackage[left=5mm,top=5mm,right=5mm,bottom=5mm]{geometry}
\\textwidth=200mm
\\usepackage[utf8]{inputenc}
\\usepackage[T1]{fontenc}
\\usepackage[T2A]{fontenc}
\\usepackage{fvextra}
\\usepackage{minted}
\\usemintedstyle{vs}
\\usepackage{makeidx}
\\usepackage[columns=1]{idxlayout}
\\makeindex
\\renewcommand{\\thesection}{\\arabic{chapter}.\\arabic{section}}
\\setcounter{chapter}{1}
\\setcounter{section}{0}
\\usepackage[tiny]{titlesec}
\\titlespacing\\chapter{0mm}{0mm}{0mm}
\\titlespacing\\section{0mm}{0mm}{0mm}
\\DeclareUnicodeCharacter{221E}{\\ensuremath{\\infty}}
\\DeclareUnicodeCharacter{FFFD}{\\ensuremath{ }}
\\usepackage{fancyhdr}
\\pagestyle{fancy}
\\fancyhf{}
\\fancyfoot[C]{\\thepage}
\\renewcommand{\\headrulewidth}{0mm}
\\renewcommand{\\footrulewidth}{0mm}
\\renewcommand{\\baselinestretch}{0.7}
\\begin{document}
\\sf
\\noindent{\\Large LinksPlatform's Platform.Disposables Class Library}
"""

# Remove auto-generated code files
find ./obj -type f -iname "*.cs" -delete

# CSharp
#find * -type f -iname '*.cs' -exec sh -c 'enconv "{}"' \;
find . -type f -iname '*.cs' | sort -b | python fmt.py

echo """
\\printindex
\\end{document}
"""
