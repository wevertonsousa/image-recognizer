using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageRecognize.API.Helpers
{
    public class FilterBlocks
    {
        public static string GetName(string[] blocks)
        {
            try
            {
                for (int i = 0; i < blocks.Count(); i++)
                {
                    if (blocks[i].ToUpper().Contains("NOME"))
                    {
                        return RemoveExcept(blocks[i + 1].ToUpper().Replace("E\n", "").Replace("\n", "").Trim());
                    }
                }

                return String.Empty;
            }
            catch
            {
                return String.Empty;
            }
        }

        public static string GetIdentidade(string[] blocks)
        {
            try
            {
                for (int i = 0; i < blocks.Count(); i++)
                {
                    if (blocks[i].ToUpper().Contains("IDENTIDADE"))
                    {
                        return RemoveExcept(blocks[i + 1].ToUpper().Replace("E\n", "").Replace("\n", "").Trim(), false, true);
                    }
                }

                return String.Empty;
            }
            catch
            {
                return String.Empty;
            }
        }

        public static string GetOrgaoEmissor(string[] blocks)
        {
            try
            {

                for (int i = 0; i < blocks.Count(); i++)
                {
                    if (blocks[i].ToUpper().Contains("IDENTIDADE"))
                    {
                        string orgaoEmissor = RemoveExcept(blocks[i + 1].ToUpper().Trim(), true, false, false, false, false, false, true);
                        var split = orgaoEmissor.Split("/");
                        for (int x = 0; x < split.Count(); x++)
                        {
                            if (split[x].Contains("SSP"))
                                return "SSP";

                            return split[i];
                        }
                    }
                }
                return String.Empty;
            }
            catch
            {
                return String.Empty;
            }
        }

        public static string GetUF(string[] blocks)
        {
            try
            {
                for (int i = 0; i < blocks.Count(); i++)
                {
                    if (blocks[i].ToUpper().Contains("IDENTIDADE"))
                    {
                        string orgaoEmissor = RemoveExcept(blocks[i + 1].ToUpper().Trim(), true, false, false, false, false, false, true);
                        var split = orgaoEmissor.Split("/");
                        for (int x = 0; x < split.Count(); x++)
                        {
                            return split[x + 1].Substring(0, 2);
                        }
                    }
                }

                return String.Empty;
            }
            catch
            {
                return String.Empty;
            }
        }

        public static string GetCPF(string[] blocks)
        {
            try
            {
                for (int i = 0; i < blocks.Count(); i++)
                {
                    if (blocks[i].ToUpper().Contains("NASCIMEN"))
                    {
                        string cpf = RemoveExcept(blocks[i + 1].ToUpper().Trim(), false, true).Substring(0, 11);
                        return cpf;
                    }
                }

                return String.Empty;
            }
            catch
            {
                return String.Empty;
            }
        }


        public static string GetDataNascimento(string[] blocks)
        {
            try
            {
                for (int i = 0; i < blocks.Count(); i++)
                {
                    if (blocks[i].ToUpper().Contains("NASCIMEN"))
                    {
                        var cpf = blocks[i + 1].Replace("B", "8");
                        var split = cpf.Split("/");
                        string dataNascimento = split[0].Substring(split[0].Length - 2) + "/" + split[1] + "/" + split[2];
                        return dataNascimento.Replace("'", "").Trim();
                    }
                }
                return String.Empty;
            }
            catch
            {
                return String.Empty;
            }
        }



        public static string GetPai(string[] blocks)
        {
            try
            {
                for (int i = 0; i < blocks.Count(); i++)
                {
                    if (blocks[i].ToUpper().Contains("FILIA"))
                    {
                        string nomePai = "";
                        nomePai = blocks[i + 1];
                        if (blocks[i + 2].Replace("\n", "") != string.Empty)
                            if (blocks[i + 2].Replace("\n", "").Length < 15)
                                nomePai += " " + blocks[i + 2].Replace("\n", "");

                        return nomePai;
                    }
                }
                return String.Empty;
            }
            catch
            {
                return String.Empty;
            }
        }

        public static string GetMae(string[] blocks)
        {
            try
            {
                for (int i = 0; i < blocks.Count(); i++)
                {
                    if (blocks[i].ToUpper().Contains("FILIA"))
                    {
                        string nomeMae = "";
                        if (blocks[i + 2].Replace("\n", "") != string.Empty)
                        {
                            if (blocks[i + 2].Replace("\n", "").Length > 15)
                                nomeMae += " " + blocks[i + 2].Replace("\n", "");
                            else
                            {
                                nomeMae += " " + blocks[i + 3].Replace("\n", "");
                            }
                        }

                        return nomeMae.Trim();
                    }
                }

                return String.Empty;
            }
            catch
            {
                return String.Empty;
            }
        }

        public static string GetNumeroRegistro(string[] blocks)
        {
            try
            {
                for (int i = 0; i < blocks.Count(); i++)
                {
                    if (blocks[i].ToUpper().Contains("VALIDADE"))
                    {
                        var validade = blocks[i + 1].Replace("B", "8");
                        var split = validade.Split(" ");
                        var numeroRegistro = split.FirstOrDefault(t => t.Length > 4);
                        if (numeroRegistro != null)
                            return numeroRegistro;
                    }
                }
                return String.Empty;
            }
            catch
            {
                return String.Empty;
            }
        }

        public static string GetValidade(string[] blocks)
        {
            try
            {
                for (int i = 0; i < blocks.Count(); i++)
                {
                    if (blocks[i].ToUpper().Contains("VALIDADE"))
                    {
                        var validade = blocks[i + 1].Replace("B", "8");
                        var split = validade.Split(" ");
                        var dataValidade = split.FirstOrDefault(t => t.Contains("/"));
                        if (dataValidade != null)
                            return dataValidade;
                    }
                }
                return String.Empty;
            }
            catch
            {
                return String.Empty;
            }
        }

        public static string GetPrimeiraHabilitacao(string[] blocks)
        {
            try
            {
                for (int i = 0; i < blocks.Count(); i++)
                {
                    if (blocks[i].ToUpper().Contains("VALIDADE"))
                    {
                        var validade = blocks[i + 1].Replace("B", "8");
                        var split = validade.Split(" ");
                        var dataValidade = split.LastOrDefault(t => t.Contains("/"));
                        if (dataValidade != null)
                            return dataValidade;
                    }
                }
                return String.Empty;
            }
            catch
            {
                return String.Empty;
            }
        }

        public static string GetDataEmissao(string[] blocks)
        {
            try
            {
                for (int i = 0; i < blocks.Count(); i++)
                {
                    if (blocks[i].ToUpper().Contains("EMISS"))
                    {
                        var emissao = blocks[i + 1].Replace("B", "8");
                        var split = emissao.Split(" ");
                        var dataEmissao = split.LastOrDefault(t => t.Contains("/"));
                        if (dataEmissao != null)
                            return dataEmissao;
                    }
                }
                return String.Empty;
            }
            catch
            {
                return String.Empty;
            }
        }

        public static string GetLocalEmissao(string[] blocks)
        {
            try
            {
                for (int i = 0; i < blocks.Count(); i++)
                {
                    if (blocks[i].ToUpper().Contains("LOCAL"))
                    {
                        return blocks[i + 1];
                    }
                }
                return String.Empty;
            }
            catch
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Returns a copy of the original string containing only the set of whitelisted characters.
        /// </summary>
        /// <param name="value">The string that will be copied and scrubbed.</param>
        /// <param name="alphas">If true, all alphabetical characters (a-zA-Z) will be preserved; otherwise, they will be removed.</param>
        /// <param name="numerics">If true, all numeric characters (0-9) will be preserved; otherwise, they will be removed.</param>
        /// <param name="dashes">If true, all dash characters (-) will be preserved; otherwise, they will be removed.</param>
        /// <param name="underlines">If true, all underscore characters (_) will be preserved; otherwise, they will be removed.</param>
        /// <param name="spaces">If true, all whitespace (e.g. spaces, tabs) will be preserved; otherwise, they will be removed.</param>
        /// <param name="periods">If true, all dot characters (".") will be preserved; otherwise, they will be removed.</param>
        public static string RemoveExcept(string value, bool alphas = false, bool numerics = false, bool dashes = false, bool underlines = false, bool spaces = false, bool periods = false, bool bars = false)
        {
            if (string.IsNullOrWhiteSpace(value)) return value;
            if (new[] { alphas, numerics, dashes, underlines, spaces, periods }.All(x => x == false)) return value;

            var whitelistChars = new HashSet<char>(string.Concat(
                alphas ? "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ" : "",
                numerics ? "0123456789" : "",
                dashes ? "-" : "",
                underlines ? "_" : "",
                periods ? "." : "",
                bars ? "/" : "",
                spaces ? " " : ""
            ).ToCharArray());

            var scrubbedValue = value.Aggregate(new StringBuilder(), (sb, @char) =>
            {
                if (whitelistChars.Contains(@char)) sb.Append(@char);
                return sb;
            }).ToString();

            return scrubbedValue;
        }


    }
}
