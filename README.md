# Audio Downloader

Simple project for downloading audio from a source and splitting it if needed.

> Built using [Braco](https://github.com/HorvatJosip/braco)

### Ideas
* Separate audio player for each time definition (start time, end time, sections)
* Cut version slider (what the end result will sound like) - collection of times || write to temp file?
* Define audio format (by default: `Author - Audio Title` where `-` is the default separator)
  * Automatically defines author and title, album needs to be specified
* Allow cutting one big file into more smaller ones (by default one album, one author, titles need to be defined alongside their timestamps)
* Allow bulk adding of authors, titles and albums (author and title by file format recursively, albums per directory)
