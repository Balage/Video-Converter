# Video-Converter
Simple GUI for ffmpeg and ffprobe.
## How to Use
Top-left corner contains the tabs for main functions of the app.
Left-side of the window is the output console, drag & drop your files here to start the process.
Right-side of the window contains settings for the conversion.
### Probe
This one uses `ffprobe.exe` to display every possible information about media and codecs in the file. There's no settings panel for this one.
### Convert Video
This one uses `ffmpeg.exe` to convert video file to a different format. Use the settings panel on the right to set the output parameters, then drag & drop the file to the output console on the left.
- Codec
	- Codecs H.264 and H.265 are supported. I did not add hardware acceleration option because it produces lower quality result, which is unsuitable for archival, which would be the main purpose of this app.
- Transform
	- `Downscale 4K to 1080`: Converts video to 1080p if source is exactly 4K in size. Works both for landscape and portrait orientation.
	- `Drop audio track`: Output will not have audio. Saves on size if audio was irrelevant or unwanted.
- Quality
	- `Constant Rate Factor`: Maintains a consistent quality level for the video. This means the output size will be bigger if the video has more movement or high frequency details, but saves a lot on blurry and mostly still videos. Default values is 23, lower value means better quality and bigger file size.
	- `Fixed size`: This one will automatically calculate quality level (at fixed bitrate) to generate video at the desired file size. This is good for chat clients where file transfer has a size limit. Result will not be an exact match, so for 10MB, might worth setting it to 9.5MB.
### Images To Video
This one uses `ffmpeg.exe` to convert a sequence if images to a video. Use the settings panel on the right to set the output parameters, then drag & drop your images files all at once to the output console on the left. Files will be used in alphabetical order!
- Image Sequence Options
	- `Base frame rate`: Frame rate of the output video (without frame generation).
	- `Generated frames`: This will add generated frames in-between the input images for a higher frame rate. Having a base frame rate of 30 with 1 generated frame will create a video at 60 FPS. Warning: frame generation is very slow! You can use the `Terminate Process` button on the toolbar if it takes more time than expected.
	- `PNG background color`: Input images can be PNG, but the output video does not have alpha channel, so this color will be used as background.

## Build Project
- Download repository.
- Open `VideoConverter.sln`.
- Right click on `VideoConverter` project and select `Publish`.
- Click `Publish` button. This creates a folder named `Release` with the app executable.
- Download pre-built ffmpeg binaries from https://www.ffmpeg.org/download.html
	- Alternative link #1: https://github.com/BtbN/FFmpeg-Builds/releases
	- Alternative link #2: https://www.gyan.dev/ffmpeg/builds/
- From the `bin` folder copy all exe and dll files to the `Release` folder.
- Run `VideoConverter.exe` and enjoy!
## License
MIT