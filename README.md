# Compressed Data Size Detector

A quick tool using KENSSharp that reports the actual size of compressed data in a file, and how many leftover trailing bytes there are. This is particularly useful for finding extra data that might accidentally be clumped in a file after the actual compressed data that the file is meant to hold.

## Usage

	CompSizeDetector [files] [-c/--comper] [-e/--enigma] [-k/--kosinski] [-k/--kosinski-mod] [-n/--nemesis] [-s/--saxman]
	
	files              - Files to detect compressed data size from
	-e/--enigma        - Treat data as compressed in Enigma
	-k/--kosinski      - Treat data as compressed in Kosinski
	-km/--kosinski-mod - Treat data as compressed in Kosinski Moduled
	-n/--nemesis       - Treat data as compressed in Nemesis
	-s/--saxman        - Treat data as compressed in Saxman