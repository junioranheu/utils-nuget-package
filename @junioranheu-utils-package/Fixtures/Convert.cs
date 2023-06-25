﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace junioranheu_utils_package.Fixtures
{
    public static class Convert
    {
        /// <summary>
        /// Converter IFormFile para bytes[];
        /// https://stackoverflow.com/questions/36432028/how-to-convert-a-file-into-byte-array-in-memory;
        /// </summary>
        public static async Task<byte[]> ConverterIFormFileParaBytes(IFormFile formFile)
        {
            await using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);

            return memoryStream.ToArray();
        }

        /// <summary>
        /// Converter Base64 para arquivo;
        /// </summary>
        public static IFormFile ConverterBase64ParaFile(string base64)
        {
            List<IFormFile> formFiles = new();
            string split = ";base64,";
            string normalizarBase64 = base64;

            if (base64.Contains(split))
            {
                normalizarBase64 = base64[(base64.IndexOf(split) + split.Length)..];
            }

            byte[] bytes = System.Convert.FromBase64String(normalizarBase64);
            MemoryStream stream = new(bytes);

            IFormFile file = new FormFile(stream, 0, bytes.Length, Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            formFiles.Add(file);

            return formFiles[0];
        }

        /// <summary>
        /// Converter Base64 para imagem;
        /// </summary>
        public static IFormFile ConverterBase64ParaImagem(string base64)
        {
            List<IFormFile> formFiles = new();

            string split = ";base64,";
            string normalizarBase64 = base64[(base64.IndexOf(split) + split.Length)..];
            byte[] bytes = System.Convert.FromBase64String(normalizarBase64);
            MemoryStream stream = new(bytes);

            IFormFile file = new FormFile(stream, 0, bytes.Length, Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            formFiles.Add(file);

            return formFiles[0];
        }

        /// <summary>
        /// Converter caminho de um arquivo para imagem;
        /// </summary>
        public static IFormFile ConverterPathParaFile(string path, string nomeArquivo, string tipoConteudo)
        {
            var fileStream = new FileStream(path, FileMode.Open);

            var formFile = new FormFile(fileStream, 0, fileStream.Length, nomeArquivo, nomeArquivo)
            {
                Headers = new HeaderDictionary(),
                ContentType = tipoConteudo
            };

            return formFile;
        }

        /// <summary>
        /// Converter caminho de um arquivo para stream;
        /// </summary>
        public static async Task<Stream?> ConverterPathParaStream(string path, int? chunkSize = 4096)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            if (!System.IO.File.Exists(path))
            {
                return null;
            }

            return await Task.FromResult(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, chunkSize.GetValueOrDefault(), FileOptions.Asynchronous));
        }

        /// <summary>
        /// Auto-sugestivo;
        /// </summary>
        public static int ConverterMegasParaBytes(double? megas)
        {
            return System.Convert.ToInt32(megas.GetValueOrDefault() * (1024 * 1024));
        }
    }
}