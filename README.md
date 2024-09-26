# HugeFileInspector

HugeFileInspector is a powerful console application written in C# (.NET Core 8) that efficiently searches and replaces text patterns in large files. It reads files line by line to minimize memory usage, making it ideal for processing massive files that might not fit entirely into memory.

## Download

[Download HugeFileInspector x64](dist/HugeFileInspector-x64.zip)

[Download HugeFileInspector x86](dist/HugeFileInspector-x86.zip)

## Features

- **Search for text patterns or regular expressions** in large files.
- **Replace found patterns** with specified text.
- **Log results** to the console or an output file.
- **Distinct mode**: Only logs unique lines that match the pattern.
- **Displays line numbers** and total match counts.
- **Error handling** for invalid inputs and option combinations.


## Usage

The application is run from the command line and accepts several options.

### Command-Line Options

- `--input` or `-i` (required): The input file to process.
- `--find` or `-f` (required): The text pattern or regular expression to search for.
- `--output` or `-o`: The output file to log results instead of the console. In replace mode, this option is **mandatory**.
- `--replace` or `-r`: The replacement text. If specified, the application will perform a replacement instead of just searching.
- `--distinct` or `-d`: Only logs distinct lines that match the pattern.

### Basic Examples

#### Search for a Pattern

Search for the pattern "error" in a log file and display matches on the console:

```bash
HugeFileInspector.exe --input "logs.txt" --find "error"
```

#### Search with Regular Expression

Search for email addresses in a file:

```bash
HugeFileInspector.exe --input "data.txt" --find "\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Z]{2,}\b"
```

#### Replace Text

Replace all occurrences of "foo" with "bar" in a file and save the result to a new file:

```bash
HugeFileInspector.exe --input "input.txt" --find "foo" --replace "bar" --output "output.txt"
```

#### Log Results to a File

Search for the word "warning" and log results to "warnings.txt":

```bash
HugeFileInspector.exe --input "logs.txt" --find "warning" --output "warnings.txt"
```

#### Use Distinct Mode

Find distinct lines containing the word "duplicate":

```bash
HugeFileInspector.exe --input "data.txt" --find "duplicate" --distinct
```

### Notes

- **Mandatory Output File in Replace Mode**: When using the `--replace` option, you must specify an output file using `--output` or `-o`.

- **Mutually Exclusive Options**: The `--replace` and `--distinct` options cannot be used together. Attempting to use both will result in an error.

- **Regular Expressions**: The `--find` option accepts regular expressions. Be sure to properly escape special characters in your shell environment.

## Error Handling

The application will display error messages in the following cases:

- **Input File Does Not Exist**: If the specified input file cannot be found.

- **Missing Output File in Replace Mode**: If you attempt to perform a replace operation without specifying an output file.

- **Invalid Option Combinations**: If you use mutually exclusive options like `--replace` and `--distinct` together.


## Building from Source

If you have cloned the repository and want to build the application yourself: Install [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) on your machine.


1. **Restore Dependencies** (if necessary):

   ```bash
   dotnet restore
   ```

2. **Build the Application**:

   ```bash
   dotnet build
   ```

3. **Run the Application**:

   ```bash
   dotnet run -- [options]
   ```

Replace `[options]` with the desired command-line options as described in the [Usage](#usage) section.

## Contributing

Contributions are welcome! Please feel free to submit a pull request or open an issue to discuss improvements or features.

## License

This project is licensed under the MIT License.

## Contact

For questions or suggestions, please contact [desmati@gmail.com](mailto:desmati@gmail.com).
