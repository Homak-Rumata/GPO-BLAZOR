using RenderingDocument = MigraDoc.DocumentObjectModel.Document;
using RenderingSection = MigraDoc.DocumentObjectModel.Section;
using RenderingTable = MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Internals;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.Serialization;
using System.Reflection.Metadata;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.DocumentObjectModel.Visitors;
using static System.Collections.Specialized.BitVector32;
using PdfSharp.Pdf.IO;
using static MigraDoc.DocumentObjectModel.Text;

namespace GPO_BLAZOR.PDFConstructor.DocumentService
{
    static class F
    {
        public static Document FA()
        {
            return new Document()
            {
                Sections = new Section[]
                {
                    new Section()
                    {
                        paragrapfs = new BaseParagraph[]
                        {
                            new Paragrapf()
                            {
                                Bold = true,
                                text = new BaseElement[]
                            {
                                new RawText { TextValue = "Договор о практической подготовке обучающихся в форме практики №" },
                                new InjectElement { Name = "ContractNumber", TextValue = "1" },
                            }
                            },
                            new Table()
                            {
                                Alignment = ParagraphAlignment.Justify,
                                Columns = new Column[] {new Column() { Priorety = 1}, new Column() { Priorety = 1} },
                                Rows = new Row[]
                                {
                                    new Row()
                                    {
                                        Alignment = ParagraphAlignment.Right,
                                        Cells = new Cell[]
                                        {
                                            new Cell()
                                            {
                                                Alignment = ParagraphAlignment.Left,
                                                Text = new Paragrapf[]
                                                {
                                                    new Paragrapf()
                                                    {
                                                        text = new BaseElement[]
                                                        {
                                                            new RawText()
                                                            {
                                                                TextValue ="г. Томск"
                                                            }
                                                        }
                                                    }
                                                }
                                            },
                                            new Cell()
                                            {
                                                Alignment = ParagraphAlignment.Right,
                                                Text = new Paragrapf[]
                                                {
                                                    new Paragrapf()
                                                    {
                                                         Alignment = ParagraphAlignment.Right,
                                                        text = new BaseElement[]
                                                        {
                                                            new RawText()
                                                            {
                                                                TextValue ="«"
                                                            },
                                                            new InjectElement()
                                                            {
                                                                Name = "ContractDay",
                                                                TextValue = DateTime.Now.Day.ToString()
                                                            },
                                                            new RawText()
                                                            {
                                                                TextValue ="» "
                                                            },
                                                            new InjectElement()
                                                            {
                                                                Name = "ContractMonth",
                                                                TextValue = $".{DateTime.Now.Month.ToString()} "
                                                            },
                                                            new InjectElement()
                                                            {
                                                                Name = "ContractYear",
                                                                TextValue = DateTime.Now.Year.ToString()
                                                            },
                                                            new RawText()
                                                            {
                                                                TextValue =" г."
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            },
                            new Paragrapf()
                            {
                                Tab = true,
                                text = new BaseElement[]
                                {
                                    new RawText {TextValue = ("Федеральное государственное автономное образовательное учреждение высшего образования «Томский государственный университет систем управления и радиоэлектроники» (ТУСУР), именуемое в дальнейшем «Университет», в лице директора центра карьеры И.А. Трубчениновой, действующего на основании доверенности от 19.09.2024 №20/3460, с одной стороны, и " )},
                                    new InjectElement() {Name = "CompanyName", TextValue="ООО ДИВИЛАЙН" } ,
                                    new RawText {TextValue = (" именуемое в дальнейшем «Профильная организация», в лице действующего на основании ")},
                                    new InjectElement() {Name="OrganizationRule", TextValue=" Устав организации "},
                                    new RawText() {TextValue = " именуемые по отдельности «Сторона», а вместе «Стороны», заключили настоящий Договор о нижеследующем."}
                                }
                            },
                            new Paragrapf()
                            {
                                Alignment= ParagraphAlignment.Center,
                                Bold = true,
                                text = new BaseElement[]
                                {
                                    new RawText {TextValue = "\n\r1. Предмет Договора"},
                                }
                            },
                            new Paragrapf()
                            {
                                Tab = true,
                                text = new BaseElement[]
                                {
                                    new RawText {TextValue = "1.1 Предметом настоящего Договора является организация практической подготовки в форме практики обучающихся (далее – практика) по направлениям подготовки/специальностям: "},
                                    new InjectElement {Name="DerictionType", TextValue="Информационная безопасность "},
                                }
                            },
                            new Paragrapf()
                            {
                                Tab=true,
                                text = new BaseElement[]
                                {
                                    new RawText {TextValue = "1.2 Образовательная программа (программы), при реализации которой (-ых) организуется практика, количество обучающихся, сроки организации практики, согласуются Сторонами и являются неотъемлемой частью настоящего Договора (приложением 1)."}
                                }
                            },
                            new Paragrapf()
                            {
                                Tab=true,
                                text = new BaseElement[]
                                {
                                    new RawText {TextValue = "1.3 Реализация практики осуществляется в помещениях профильной организации, перечень которых согласуется Сторонами и является неотъемлемой частью настоящего Договора (приложение 2)."}
                                }
                            },
                            new Paragrapf()
                            {
                                Bold = true,
                                Alignment = ParagraphAlignment.Center,
                                text = new BaseElement[]
                                {
                                    new RawText {TextValue = "2. Права и обязанности Сторон"}
                                }
                            },
                            new Paragrapf()
                            {
                                Tab = true,
                                text = new BaseElement[]
                                {
                                    new RawText {TextValue = "2.1  Университет обязан: "}
                                }
                            },
                            new Paragrapf()
                            {
                                Tab=true,
                                text = new BaseElement[]
                                {
                                    new RawText {TextValue = "2.1.1 Не позднее, чем за 10 рабочих дней до начала практики представить в Профильную организацию поименные списки обучающихся, направляемых для прохождения практики, программу практики, график прохождения практик."}
                                }
                            },
                            new Paragrapf()
                            {
                                Tab=true,
                                text = new BaseElement[]
                                {
                                    new RawText {TextValue = "2.1.2 Назначить руководителя по практической подготовке от Университета, который: "}
                                }
                            },
                            new Paragrapf()
                            {
                                Tab=true,
                                text = new BaseElement[]
                                {
                                    new RawText {TextValue = "– обеспечивает организацию практики обучающихся;"}
                                }
                            },
                            new Paragrapf()
                            {
                                Tab=true,
                                text = new BaseElement[]
                                {
                                    new RawText {TextValue = "– контролирует участие обучающихся в выполнении определенных видов работ, связанных с будущей профессиональной деятельностью;"}
                                }
                            },
                            new Paragrapf()
                            {
                                Tab=true,
                                text = new BaseElement[]
                                {
                                    new RawText {TextValue = "– оказывает методическую помощь обучающимся при выполнении определенных видов работ, связанных с будущей профессиональной деятельностью;"}
                                }
                            },
                            new Paragrapf()
                            {
                                Tab=true,
                                text = new BaseElement[]
                                {
                                    new RawText {TextValue = "– несет ответственность совместно с руководителем практики от профильной организации за реализацию программы практики, за жизнь и здоровье обучающихся и работников Университета, соблюдение ими правил противопожарной безопасности, правил охраны труда, техники безопасности и санитарно-эпидемиологических правил и гигиенических нормативов."}
                                }
                            },
                            new Paragrapf()
                            {
                                Tab = true,
                                text = new BaseElement[]
                                {
                                    new RawText {TextValue = "– несет ответственность совместно с руководителем практики от профильной организации за реализацию программы практики, за жизнь и здоровье обучающихся и работников Университета, соблюдение ими правил противопожарной безопасности, правил охраны труда, техники безопасности и санитарно-эпидемиологических правил и гигиенических нормативов."}
                                }
                            },
                            new Paragrapf()
                            {
                                Tab = true,
                                text = new BaseElement[]
                                {
                                    new RawText {TextValue = "2.1.3 При смене руководителя практики в 7-дневный срок сообщить об этом Профильной организации."}
                                }
                            },
                            new Paragrapf()
                            {
                                Tab = true,
                                text = new BaseElement[]
                                {
                                    new RawText {TextValue = "2.1.4 Направить обучающихся в Профильную организацию для освоения программы практики."}
                                }
                            },
                            new Paragrapf()
                            {
                                Tab = true,
                                text = new BaseElement[]
                                {
                                    new RawText {TextValue = "2.1.5 Расследовать и учитывать несчастные случаи, если они произойдут со обучающимися в период прохождения практической подготовки на территории профильной организации."}
                                }
                            },
                            new Paragrapf()
                            {
                                Tab = true,
                                text = new BaseElement[]
                                {
                                    new RawText {TextValue = "\r"}
                                }
                            },
                            new Paragrapf()
                            {
                                Tab = true,
                                text = new BaseElement[]
                                {
                                    new RawText {TextValue = "2.2  Профильная организация обязана: "}
                                }
                            },
                            new Paragrapf()
                            {
                                Tab = true,
                                text = new BaseElement[]
                                {
                                    new RawText {TextValue = "2.2.1 Создать условия для реализации программы практики, предоставить оборудование и технические средства обучения в объеме, позволяющем выполнять определенные виды работ, связанные с будущей профессиональной деятельностью обучающихся."}
                                }
                            },
                            new Paragrapf()
                            {
                                Tab = true,
                                text = new BaseElement[]
                                {
                                    new RawText {TextValue = "2.2.2 Назначить руководителем практики от профильной организации "},
                                    new InjectElement {Name="OrganiztionLeaderName", TextValue="Иванов Иван Иванович"},
                                    new RawText {TextValue = ", соответствующего требованиям трудового законодательства Российской Федерации о допуске к педагогической деятельности (Приложение 3). "}
                                }
                            },
                            new Paragrapf()
                            {
                                Tab = true,
                                text = new BaseElement[]
                                {
                                    new RawText {TextValue = "2.2.3 При смене лица руководителя практики от профильной организации в 7-дневный срок сообщить об этом Университету."}
                                }
                            },
                            new Paragrapf()
                            {
                                Tab = true,
                                text = new BaseElement[]
                                {
                                    new RawText {TextValue = "2.2.4 Обеспечить реализацию программы практики со стороны профильной организации;"}
                                }
                            },
                            new Paragrapf()
                            {
                                Tab = true,
                                text = new BaseElement[]
                                {
                                    new RawText {TextValue = "2.2.5 Обеспечить безопасные условия реализации программы практики, выполнение правил противопожарной безопасности, правил охраны труда, техники безопасности и санитарно-эпидемиологических правил и гигиенических нормативов;"}
                                }
                            },
                            new Paragrapf()
                            {
                                Tab = true,
                                text = new BaseElement[]
                                {
                                    new RawText {TextValue = "2.2.6 Проводить оценку условий труда на рабочих местах, используемых при реализации программы практики;"}
                                }
                            },
                            new Paragrapf()
                            {
                                Tab = true,
                                text = new BaseElement[]
                                {
                                    new RawText {TextValue = "2.2.7 Ознакомить обучающихся с правилами внутреннего трудового распорядка профильной организации и иными необходимыми локальными нормативными актами;"}
                                }
                            },
                            new Paragrapf()
                            {
                                Tab = true,
                                text = new BaseElement[]
                                {
                                    new RawText {TextValue = "2.2.8 Провести инструктаж обучающихся по охране труда и технике безопасности"}
                                }
                            },
                            new Paragrapf()
                            {
                                Tab = true,
                                text = new BaseElement[]
                                {
                                    new RawText {TextValue = "2.2.9 Предоставить обучающимся и руководителю практики от Университета возможность пользоваться помещениями Профильной организации, согласованными Сторонами (приложение 2 к настоящему Договору), а также находящимися в них оборудованием и техническими средствами обучения."}
                                }
                            },
                            new Paragrapf()
                            {
                                Tab = true,
                                text = new BaseElement[]
                                {
                                    new RawText {TextValue = "2.2.10 Обо всех случаях нарушения обучающимися правил внутреннего трудового распорядка, охраны труда и техники безопасности сообщить руководителю практики от университета;"}
                                }
                            },
                            new Paragrapf()
                            {
                                Tab = true,
                                text = new BaseElement[]
                                {
                                    new RawText {TextValue = "2.2.11 Расследовать и учитывать несчастные случаи, если они произойдут с обучающимися в период прохождения практики в Профильной организации в соответствии с Положением о расследовании и учёте несчастных случаев на производстве."}
                                }
                            },
                            new Paragrapf()
                            {
                                Tab = true,
                                text = new BaseElement[]
                                {
                                    new RawText {TextValue = "2.2.12 Не допускать использования обучающихся на должностях, не предусмотренных программой практики и не имеющих отношения к направлению подготовки/специальности обучающихся."}
                                }
                            },
                            new Paragrapf()
                            {
                                Tab = true,
                                text = new BaseElement[]
                                {
                                    new RawText {TextValue = "\r"}
                                }
                            },
                            new Paragrapf()
                            {
                                Tab = true,
                                text = new BaseElement[]
                                {
                                    new RawText {TextValue = "Университет имеет право:"}
                                }
                            },
                            new Paragrapf()
                            {
                                Tab = true,
                                text = new BaseElement[]
                                {
                                    new RawText {TextValue = "Требовать от обучающихся соблюдения правил внутреннего трудового распорядка, охраны труда и техники безопасности, режима конфиденциальности, принятого в профильной организации, предпринимать необходимые действия, направленные на предотвращение ситуации, способствующей разглашению конфиденциальной информации."}
                                }
                            },
                            new Paragrapf()
                            {
                                Tab = true,
                                text = new BaseElement[]
                                {
                                    new RawText {TextValue = "В случае установления факта нарушения обучающимися своих обязанностей в период организации практики, режима конфиденциальности приостановить реализацию программы практики в отношении конкретного обучающегося."}
                                }
                            },
                            new Paragrapf()
                            {
                                Bold = true,
                                Alignment= ParagraphAlignment.Center,
                                text = new BaseElement[]
                                {
                                    new RawText {TextValue = "3 Срок действия договора"}
                                }
                            },
                            new Paragrapf()
                            {
                                Tab = true,
                                text = new BaseElement[]
                                {
                                    new RawText {TextValue = "3.1 Настоящий Договор вступает в силу после его подписания и действует до полного исполнения Сторонами обязательств."}
                                }
                            },
                            new Paragrapf()
                            {
                                Bold = true,
                                Alignment = ParagraphAlignment.Center,
                                text = new BaseElement[]
                                {
                                    new RawText {TextValue = "4. Заключительные положения"}
                                }
                            },
                            new Paragrapf()
                            {
                                Tab = true,
                                text = new BaseElement[]
                                {
                                    new RawText {TextValue = "4.1 Все споры, возникающие между Сторонами по настоящему Договору, разрешаются Сторонами в порядке, установленном законодательством Российской Федерации."}
                                }
                            },
                            new Paragrapf()
                            {
                                Tab = true,
                                text = new BaseElement[]
                                {
                                    new RawText {TextValue = "4.2 Изменение настоящего Договора осуществляется по соглашению Сторон в письменной форме в виде дополнительных соглашений к настоящему Договору, которые являются его неотъемлемой частью."}
                                }
                            },
                            new Paragrapf()
                            {
                                Tab = true,
                                text = new BaseElement[]
                                {
                                    new RawText {TextValue = "4.3 Настоящий Договор составлен в двух экземплярах, по одному для каждой из Сторон. Все экземпляры имеют одинаковую юридическую силу."}
                                }
                            },
                            new Paragrapf()
                            {
                                Bold = true,
                                Alignment = ParagraphAlignment.Center,
                                KeepWithNext = true,
                                text = new BaseElement[]
                                {
                                    new RawText {TextValue = "5. Адреса, реквизиты и подписи Сторон"}
                                }
                            },
                            new Table()
                            {
                                KeepTogether = true,
                                KeepWithNext = true,
                                Columns = new Column[] {new Column() { Priorety = 1}, new Column() { Priorety = 1 } },
                                Rows = new Row[]
                                {
                                    new Row()
                                    {
                                        KeepWith = 4,
                                        KeepWithNext = true,
                                        Cells = new Cell[]
                                        {
                                            new Cell()
                                            {
                                                KeepWithNext = true,
                                                Text = new Paragrapf[]
                                                {
                                                    new Paragrapf()
                                                    {
                                                        KeepWithNext = true,
                                                        Bold = true,
                                                        text = new BaseElement[]
                                                        {
                                                            new RawText()
                                                            {
                                                                TextValue = "Университет: "
                                                            }
                                                        }
                                                    },
                                                    new Paragrapf()
                                                    {
                                                        KeepWithNext = true,
                                                        text = new BaseElement[]
                                                        {
                                                            new RawText()
                                                            {
                                                                TextValue = "Федеральное государственное автономное образовательное учреждение высшего образования «Томский государственный университет систем управления и радиоэлектроники»"
                                                            }
                                                        }
                                                    },
                                                },
                                            },
                                            new Cell()
                                            {
                                                KeepWithNext = true,
                                                Text = new Paragrapf[]
                                                {

                                                    new Paragrapf()
                                                    {
                                                        KeepWithNext = true,
                                                        Bold = true,
                                                        text = new BaseElement[]
                                                        {
                                                            new RawText()
                                                            {
                                                                TextValue = "Профильная организация: "
                                                            }
                                                        }
                                                    },
                                                    new Paragrapf()
                                                    {
                                                        KeepWithNext = true,
                                                        text = new BaseElement[]
                                                        {
                                                            new InjectElement()
                                                            {
                                                                TextValue = "ООО ДиВиЛайн",
                                                                Name = "FactoryName"
                                                            }
                                                        }
                                                    },
                                                },
                                            }
                                        }
                                    },
                                    new Row()
                                    {
                                        KeepWithNext = true,
                                        Cells = new Cell[]
                                        {
                                            new Cell()
                                            {
                                                KeepWithNext = true,
                                                Text = new Paragrapf[]
                                                {
                                                    new Paragrapf()
                                                    {
                                                        KeepWithNext = true,
                                                        text = new BaseElement[]
                                                        {
                                                            new RawText()
                                                            {
                                                               TextValue = "Адрес: 634050, г. Томск, пр. Ленина, 40"
                                                            }
                                                        }
                                                    },
                                                }
                                            },
                                            new Cell()
                                            {
                                                KeepWithNext = true,
                                                Text = new Paragrapf[]
                                                {
                                                    new Paragrapf()
                                                    {
                                                        KeepWithNext = true,
                                                        text = new BaseElement[]
                                                        {
                                                            new RawText()
                                                            {
                                                                TextValue = "Адресс: "
                                                            },
                                                            new InjectElement()
                                                            {
                                                                TextValue = "634050, г. Томск, ул. Вершинина, 36",
                                                                Name = "FactoryLocation"
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new Row()
                                    {
                                        KeepWithNext = true,
                                        Cells = new Cell[]
                                        {
                                            new Cell()
                                            {
                                                KeepWithNext = true,
                                                Text = new Paragrapf[]
                                                {
                                                     new Paragrapf()
                                                    {
                                                        KeepWithNext = true,
                                                        text = new BaseElement[]
                                                        {
                                                            new RawText()
                                                            {
                                                               TextValue = "Директор центра карьеры "
                                                            }
                                                        }
                                                    },
                                                }
                                            },
                                            new Cell()
                                            {
                                                KeepWithNext = true,
                                                Text = new Paragrapf[]
                                                {
                                                    new Paragrapf()
                                                    {
                                                        KeepWithNext = true,
                                                        text = new BaseElement[]
                                                        {
                                                            new InjectElement()
                                                            {
                                                                TextValue = "Директор",
                                                                Name = "FactoryRank"
                                                            }
                                                        }
                                                    },
                                                }
                                            }
                                        }
                                    },
                                    new Row()
                                    {
                                        KeepWithNext = true,
                                        Cells = new Cell[]
                                        {
                                            new Cell()
                                            {
                                                Text = new Paragrapf[]
                                                {
                                                    new Paragrapf()
                                                    {
                                                        text = new BaseElement[]
                                                        {
                                                            new RawText()
                                                            {
                                                                TextValue = "____________"
                                                            },
                                                            new RawText()
                                                            {
                                                                TextValue = " И.А. Трубченинова"
                                                            }
                                                        }
                                                    },
                                                    new Paragrapf()
                                                    {
                                                        Size = 10,
                                                        Tab = true,
                                                        text = new BaseElement[]
                                                        {
                                                            new RawText()
                                                            {
                                                               SpecialLine = true,
                                                               SuperScript = true,
                                                               Bold = false,
                                                               TextValue = "(Подпись)"
                                                            }
                                                        }
                                                    },
                                                }
                                            },
                                            new Cell()
                                            {
                                                Text = new Paragrapf[]
                                                {
                                                    new Paragrapf()
                                                    {
                                                        text = new BaseElement[]
                                                        {
                                                            new RawText()
                                                            {
                                                                TextValue = "____________"
                                                            },
                                                            new InjectElement()
                                                            {
                                                                TextValue = "Иванов Иван Иванович",
                                                                Name = "FactoryLeaderName"
                                                            }
                                                        }
                                                    },
                                                    new Paragrapf()
                                                    {
                                                        Tab = true,
                                                        Size = 10,
                                                        text = new BaseElement[]
                                                        {
                                                            new RawText()
                                                            {
                                                               SpecialLine = true,
                                                               SuperScript = true,
                                                               Bold = false,
                                                               TextValue = "(Подпись)"
                                                            }
                                                        }
                                                    },
                                                }
                                            }
                                        }
                                    },
                                    new Row()
                                    {
                                        KeepWithNext = true,
                                        Cells = new Cell[]
                                        {
                                            new Cell()
                                            {
                                                KeepWithNext = true,
                                                KeepTogether = true,
                                                Text = new Paragrapf[]
                                                {
                                                    new Paragrapf()
                                                    {
                                                        Size = 10,
                                                        KeepTogether = true,
                                                        KeepWithNext = true,
                                                        text = new BaseElement[]
                                                        {
                                                             new RawText()
                                                            {
                                                               SpecialLine = true,
                                                               SuperScript = true,
                                                               Bold = false,
                                                               TextValue = "\n(печать университета)"
                                                            }
                                                        }
                                                    }
                                                }
                                            },
                                            new Cell()
                                            {
                                                KeepWithNext = true,
                                                KeepTogether = true,
                                                Text = new Paragrapf[]
                                                {
                                                    new Paragrapf()
                                                    {
                                                        Size = 10,
                                                        KeepTogether = true,
                                                        KeepWithNext = true,
                                                        text = new BaseElement[]
                                                        {
                                                             new RawText()
                                                            {
                                                               SpecialLine = true,
                                                               SuperScript = true,
                                                               Bold = false,
                                                               TextValue = "\n(печать проф. организации, при наличии)"
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    },
                    new Section()
                    {
                        paragrapfs = new BaseParagraph[]
                        {
                            new Paragrapf()
                            {
                                Alignment = ParagraphAlignment.Right,
                                text = new BaseElement[]
                                {
                                    new RawText()
                                    {
                                        TextValue = "Приложение № 1\r\nк договору о практической подготовке обучающихся \r\nв форме практики\r\nот "
                                    },
                                    new InjectElement()
                                    {
                                        TextValue = DateTime.Now.ToString(),
                                        Name = "DateTimeStart"
                                    },
                                    new RawText()
                                    {
                                        TextValue = " № "
                                    },
                                    new InjectElement()
                                    {
                                        TextValue = "1",
                                        Name = "ContractNumber"
                                    },
                                }
                            },
                            new Paragrapf()
                            {
                                Alignment = ParagraphAlignment.Center,
                                Bold= true,
                                text = new BaseElement[]
                                {
                                    new RawText()
                                    {
                                        TextValue = "План-график прохождения практической подготовки в форме практики обучающихся в профильной организации"
                                    }
                                }
                            },
                            new Paragrapf()
                            {

                            },
                            new Table()
                            {
                                Alignment = ParagraphAlignment.Left,
                                TableBorders = new Borders()
                                {
                                    Visible = true,
                                    DistanceFromRight = 0,
                                    DistanceFromLeft = 0,
                                },
                                Columns = new Column[]
                                {
                                    new Column() {Priorety = 25},
                                    new Column() {Priorety = 25},
                                    new Column() {Priorety = 16},
                                    new Column() {Priorety = 16},
                                    new Column() {Priorety = 18},
                                },
                                Rows = new Row[]
                                {
                                    new Row()
                                    {
                                        Cells = new Cell[]
                                        {
                                            new Cell()
                                            {
                                                Text = new Paragrapf[]
                                                {
                                                    new Paragrapf()
                                                    {
                                                        text = new BaseElement[]
                                                        {
                                                            new RawText()
                                                            {
                                                                TextValue =  "Об|ра|зо|ва|тель|на|я прог|рам|ма".Replace('|','\u00AD')
                                                            },
                                                        }
                                                    }
                                                }

                                            },
                                            new Cell()
                                            {
                                                Text = new Paragrapf[]
                                                {
                                                    new Paragrapf()
                                                    {
                                                        text = new BaseElement[]
                                                        {
                                                            new RawText()
                                                            {
                                                                TextValue = "Ком|по|нент(-ы) об|ра|зо|ва|тель|ной прог|рам|мы".Replace('|','\u00AD')
                                                            }
                                                        }
                                                    }
                                                }

                                            },
                                            new Cell()
                                            {
                                                Text = new Paragrapf[]
                                                {
                                                    new Paragrapf()
                                                    {
                                                        Alignment = ParagraphAlignment.Left,
                                                        text = new BaseElement[]
                                                        {
                                                            new RawText()
                                                            {
                                                                TextValue = "Ко|ли|чест|во о|бу|ча|ю|щих|ся".Replace('|','\u00AD')
                                                            }
                                                        }
                                                    }
                                                }

                                            },
                                            new Cell()
                                            {
                                                Text = new Paragrapf[]
                                                {
                                                    new Paragrapf()
                                                    {
                                                        text = new BaseElement[]
                                                        {
                                                            new RawText()
                                                            {
                                                                TextValue =  "Ф.И.О., курс, груп|па".Replace('|','\u00AD')
                                                            }
                                                        }
                                                    }
                                                }

                                            },
                                            new Cell()
                                            {
                                                Text = new Paragrapf[]
                                                {
                                                    new Paragrapf()
                                                    {
                                                        text = new BaseElement[]
                                                        {
                                                            new RawText()
                                                            {
                                                                TextValue = "Сро|ки ор|га|ни|за|ции прак|ти|чес|кой под|го|тов|ки".Replace('|','\u00AD')
                                                            }
                                                        }
                                                    }
                                                }

                                            }
                                        }
                                    },
                                    new Row()
                                    {
                                        Cells = new Cell[]
                                        {
                                            new Cell()
                                            {

                                            }
                                        }
                                    }
                                }
                                
                            }
                        }
                    }
                }

            };
        }
    }

    public interface IElement
    {
        //void Render();
    }

    public interface IElement<T> : IElement
    {
        void Render (in T element);
    }

    public interface IDocument : IElement;
    public interface ISections : IElement<RenderingDocument>;


    public interface IParagraph : IElement<RenderingSection>;

    
    public interface IBaseElement : IElement<Paragraph>
    {
        string TextValue { get; set; }
    }
    public interface IInjectValue : IBaseElement
    {
    }
    public interface IText: IBaseElement
    {
    }

    public record struct Margins
    { 
        public Margins()
        {

        }
        public Margins(int right, int left, int top, int bottom)
        {
            Right = right;
            Left = left;
            Top = top;
            Bottom = bottom;
        }
        [XmlAttribute("right")]
        public int Right { get; init; }
        [XmlAttribute("left")]
        public int Left { get; init; }
        [XmlAttribute("top")]
        public int Top { get; init; }
        [XmlAttribute("bottom")]
        public int Bottom { get; init; }
    }



    [DataContract]
    //[XmlRoot(Namespace = "Templates", ElementName = "Document", IsNullable = false, DataType = "string")]
    [XmlInclude(typeof(Paragrapf))]
    public struct Document: IDocument
    {
        public Document()
        {

        }
        [XmlArray]
        public Section[] Sections { get; set; }
        public Margins margin { get; set; } = new(45, 60, 60, 60);

        public RenderingDocument Render()
        {
            var document = new RenderingDocument();
            foreach (var section in Sections)
            {
                section.Render(document);
            }

            return document;
        }
    }

    public record struct Section: ISections
    {
        public Section()
        {

        }
        [XmlArray]
        [XmlArrayItem("Paragrapf", typeof(Paragrapf))]
        [XmlArrayItem("Table", typeof(Table))]
        public BaseParagraph[] paragrapfs { get; set; }

        public void Render(in RenderingDocument document)
        {
            var section = document.AddSection();
            document.AddStyle("OS TUSUR", "normal");
            section.PageSetup = document.DefaultPageSetup.Clone();
            section.PageSetup.PageFormat = PageFormat.A4;
            foreach (IParagraph temp in paragrapfs)
            {
                temp.Render(section);
            }
        }

    }


    [JsonArray]
    [Serializable]
    [JsonDerivedType(typeof(TextFormat), "TextFormat")]
    public abstract record class TextFormat
    {

        [XmlAttribute]
        public int Size { get => _size ?? 14; set => _size = value; }
        protected int? _size;
        [XmlAttribute]
        public bool Bold { get => _bold ?? false; set => _bold = value; }
        protected bool? _bold;

        [XmlAttribute]
        public Underline Underline { get => _underline ?? Underline.None; set => _underline = value; }
        protected Underline? _underline;

        [XmlAttribute]
        public bool Italic { get => _italic ?? false; set => _italic = value; }
        protected bool? _italic;
    }

    [JsonArray]
    [Serializable]
    [JsonDerivedType(typeof(FormatedElement), "FormatedElement")]
    public abstract record class FormatedElement: TextFormat
    {
        [XmlAttribute]
        public ParagraphAlignment Alignment { get => _alignment ?? ParagraphAlignment.Justify; set => _alignment = value; }
        private ParagraphAlignment? _alignment;
        public Borders Borders { get; set; }
        [XmlAttribute]
        public bool KeepWithNext { get; set; }
        [XmlAttribute]
        public bool KeepTogether { get; set; }

        protected ParagraphFormat SetParametress(ParagraphFormat paragraphFormat)
        {
            paragraphFormat.KeepWithNext = KeepWithNext;
            paragraphFormat.KeepTogether = KeepTogether;
            paragraphFormat.Font.Name = "Times";
            paragraphFormat.Font.Bold = Bold;
            paragraphFormat.Font.Size = Size;
            paragraphFormat.Font.Underline = Underline;
            paragraphFormat.Font.Italic = Italic;
            if (Borders is not null)
            paragraphFormat.Borders = Borders;
            paragraphFormat.Alignment = Alignment;
            return paragraphFormat;
        }
    }

    [JsonArray]
    [Serializable]
    [JsonDerivedType(typeof(Paragrapf), "Paragrapf")]
    [JsonDerivedType(typeof(Table), "Table")]
    public abstract record class BaseParagraph : FormatedElement, IParagraph
    {
        public abstract void Render(in RenderingSection element);
        public abstract void Render(in RenderingTable.Cell element);
        public abstract void Render(in RenderingTable.Cell element, Unit with);
    }


    [Serializable]
    public record class Paragrapf: BaseParagraph, IParagraph
    {
        public Paragrapf()
        {

        }
        [XmlArray("Text")]
        [XmlArrayItem("InjectElement", typeof(InjectElement))]
        [XmlArrayItem("Text", typeof(RawText))]
        public BaseElement[] text { get; set; }
        
        [XmlAttribute]
        public int SpaceNum { get=>spaceNum; set=>spaceNum=value; }
        private int spaceNum = 0;

        [XmlAttribute]
        public bool Tab { get => tab; set => tab = value; }
        private bool tab = false;

        public override void Render(in RenderingSection section)
        {
            var paragraph = section.AddParagraph();
            SetParametress(paragraph.Format);
            paragraph.AddSpace(SpaceNum);
            if (tab) paragraph.AddTab();
            if (text is not null)
                foreach (IBaseElement temp in text)
                {
                    temp.Render(paragraph);
                }
        }

        public override void Render(in RenderingTable.Cell section)
        {
            var paragraph = section.AddParagraph();
            SetParametress(paragraph.Format);
            paragraph.AddSpace(SpaceNum);
            if (tab) paragraph.AddTab();
            if (text is not null)
                foreach (IBaseElement temp in text)
                {
                    temp.Render(paragraph);
                }
        }
        public override void Render(in RenderingTable.Cell section, Unit With)
        {
            var paragraph = section.AddParagraph();
            SetParametress(paragraph.Format);
            paragraph.Format.LeftIndent = With;
            paragraph.Format.RightIndent = With;

            paragraph.AddSpace(SpaceNum);
            if (tab) paragraph.AddTab();
            if (text is not null)
                foreach (IBaseElement temp in text)
                {
                    temp.Render(paragraph);
                }
        }




    }

    public record class Table: BaseParagraph
    {
        public Table()
        {

        }
        public Borders TableBorders { get; set; }
        public Row Head { get; set; }

        [XmlArray]
        public Row[] Rows { get; set; }

        [XmlArray]
        public Column[] Columns { get; set; }
        public override void Render(in RenderingSection section)
        {
            var Table = section.AddTable();
            Table.KeepTogether = KeepTogether;
            SetParametress(Table.Format);
            Table.Borders = TableBorders;
            Unit width = (Table.Section.PageSetup.PageWidth - Table.Section.PageSetup.LeftMargin - Table.Section.PageSetup.RightMargin) / Columns.Sum(x=>x.Priorety);
            foreach (Column column in Columns)
                column.Render(Table, width*column.Priorety);
            if (Head is not null)
            {
                Head.Render(Table, true);
            }
            foreach (Row row in Rows)
                row.Render(Table);
        }
        public override void Render(in RenderingTable.Cell section)
        {
            TableRender (section.AddTextFrame().AddTable());
        }

        public override void Render(in RenderingTable.Cell section, Unit with)
        {

            var Table = section.AddTextFrame().AddTable();
            Table.Format.RightIndent = with;
            Table.Format.LeftIndent = with;
            TableRender(Table);

        }

        public void TableRender (RenderingTable.Table Table)
        {

            SetParametress(Table.Format);
            Unit width = Table.Section.PageSetup.PageWidth / Columns.Length;
            Head.Render(Table, true);
            foreach (Column column in Columns)
                column.Render(Table, width);
        }
    }

    public record class  Row: FormatedElement
    {
        public Row()
        {

        }
        
        public Cell[] Cells { get; set; }
        [XmlAttribute]
        public int KeepWith { get; set; }
        public void Render(in RenderingTable.Table section,bool isHead = false)
        {
            var row = section.AddRow();
            row.KeepWith = KeepWith;
            SetParametress(row.Format);
            row.HeadingFormat = isHead;
            for (int i = 0; Cells.Length > i; i++)
            {
                Cells[i].Render(row.Cells[i]);
            }
        }
    }

    public record class Column: FormatedElement
    {
        public Column()
        {

        }
        [XmlAttribute]
        public int Priorety { get; set; }
        public Cell[] Cells { get; set; }

        public void Render(in RenderingTable.Table section, Unit width, bool isHead = false)
        {
            RenderingTable.Column column;
            var dob = width.Value;
            if (!dob.Equals(double.NaN))
                column = section.AddColumn(width);
            else
                column = section.AddColumn();
            SetParametress(column.Format);
            column.HeadingFormat = isHead;
            if (Cells is not null)
            {
                for (int i = 0; Cells.Length > i; i++)
                    Cells[i].Render(column[i]);
            }
            return;

        }
    }

    public record class Cell: FormatedElement
    {
        public Cell()
        {

        }
        public Paragrapf[] Text { get; set; }
        public void Render(in RenderingTable.Cell cell)
        {
            if (cell is not null)
            {
                SetParametress(cell.Format);
                foreach (var text in Text)
                    text.Render(cell);
            }
            return;
        }

        public void Render(in RenderingTable.Cell cell, Unit with)
        {
            if (cell is not null)
            {
                SetParametress(cell.Format);
                foreach (var text in Text)
                    text.Render(cell, with);
            }
            return;
        }

    }



    [JsonDerivedType(typeof(InjectElement), "InjectElement")]
    [JsonDerivedType(typeof(RawText), "Text")]
    public abstract record class BaseElement : TextFormat, IBaseElement
    {
        public BaseElement()
        {

        }
        [XmlAttribute]
        public bool SpecialLine;
        [XmlAttribute]
        public bool SuperScript;
        [XmlAttribute]
        public bool SubScript;
        [XmlText]
        public abstract string TextValue { get; set; }
        public void Render(in Paragraph paragraph)
        {
            var formatedText = paragraph.AddFormattedText($"{TextValue}");
            if (SpecialLine)
            {
                formatedText.Italic = Italic;
                formatedText.Underline = Underline;
                formatedText.Bold = Bold;
                formatedText.Subscript = SubScript;
                formatedText.Superscript = SuperScript;
            }
        }
    }


    
    public record class InjectElement: BaseElement, IInjectValue
    {
        public InjectElement()
        {

        }
        [DataMember]
        [XmlAttribute]
        public string Name { get; set; }
        [XmlText]
        public override string TextValue { get; set; }
    }

    


    
    public record class RawText : BaseElement, IText
    {
        public RawText()
        {

        }
        [XmlText]
        public override string TextValue { get; set; }

    }
}
