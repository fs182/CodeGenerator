using CodeGenerator.Infrastructure.Context.Models;
using System.Text;

namespace CodeGenerator.Infrastructure.Templates.CleanArquitecture.UI.React
{
    public static class Component
    {
        public static void WriteComponents(Project project, Table table)
        {
            if (!Directory.Exists(Path.Combine(project.UIComponentsPath)))
                Directory.CreateDirectory(Path.Combine(project.UIComponentsPath));
            using StreamWriter outputFile = new(Path.Combine(project.UIComponentsPath, string.Concat(table.TableName, ".js")), false, Encoding.UTF8);
            var pk = table.Columns.First(f => f.IsPrimaryKey);
            outputFile.WriteLine($"import React from 'react';");
            outputFile.WriteLine($"import axios from 'axios';");
            outputFile.WriteLine($"import {table.TableName}Item from '../../pages/catalogs/{table.TableName}Item';");
            outputFile.WriteLine($"import SendIcon from '@mui/icons-material/Send';");
            outputFile.WriteLine("import {Typography,Pagination, Grid, Stack, Fab, Button, Dialog, DialogActions, DialogContent, TextField, useTheme, Box } from '@mui/material';");
            outputFile.WriteLine("import Autocomplete from '@mui/material/Autocomplete';");
            outputFile.WriteLine($"import AddCircleIcon from '@mui/icons-material/AddCircle';");
            outputFile.WriteLine("import {useTranslation} from 'react-i18next';");
            outputFile.WriteLine("import {API_URL,ROWS_OF_PAGE} from '../../utils/constants/paths';");
            outputFile.WriteLine($"import JumboCardQuick from '@jumbo/components/JumboCardQuick';");
            outputFile.WriteLine($"import Div from '@jumbo/shared/Div';");
            outputFile.WriteLine($"import useSwalWrapper from '@jumbo/vendors/sweetalert2/hooks';");
            outputFile.WriteLine($"");
            outputFile.WriteLine(string.Concat("const ", table.TableName, " = () => {"));
            outputFile.WriteLine("    const { t } = useTranslation();");
            outputFile.WriteLine("    const formRef = React.useRef();");
            outputFile.WriteLine($"    const [{Helper.GetCamel(table.TableName)}s, set{table.TableName}s] = React.useState(null);");

            var foreings = table.Columns.Where(f => f.IsForeignKey && f.ColumnName != "AuditoriaId").ToArray();
            string prefixFk = "";
            int countFk = 0;
            for (var i = 0; i <= foreings.Count() - 1; i++)
            {
                if (table.Columns.Count(f => f.TableTarget == foreings[i].TableTarget) > 1)
                {
                    countFk++;
                    prefixFk = countFk.ToString();
                }
                outputFile.WriteLine($"    const [{Helper.GetCamel(foreings[i].TableTarget)}{prefixFk}s, set{foreings[i].TableTarget}{prefixFk}s] = React.useState(false);");
                outputFile.WriteLine(string.Concat("    const [", Helper.GetCamel(foreings[i].TableTarget), prefixFk, ", set", foreings[i].TableTarget, prefixFk, "] = React.useState({ });"));
                outputFile.WriteLine($"    const [call{foreings[i].TableTarget}{prefixFk}, setCall{foreings[i].TableTarget}{prefixFk}] = React.useState(false);");
                prefixFk = "";
            }

            outputFile.WriteLine($"    const [totalPages, setTotalPages] = React.useState(0);");
            outputFile.WriteLine($"    const [currentPage, setCurrentPage] = React.useState(1);");
            outputFile.WriteLine($"    const [open, setOpen] = React.useState(false);");
            outputFile.WriteLine($"    const [createMode, setCreateMode] = React.useState(true);");
            outputFile.WriteLine($"    const [createItem, setCreateItem] = React.useState(false);");
            outputFile.WriteLine($"    const [dialogTitle, setDialogTitle] = React.useState('');");
            outputFile.WriteLine($"    const [dialogButtonText, setDialogButtonText] = React.useState('');");
            outputFile.WriteLine("    const [refItem, setRefItem] = React.useState({});");
            foreach (var c in table.Columns.Where(f => !f.IsIdentity && f.ColumnName != "AuditoriaId"))
                outputFile.WriteLine($"    const [{Helper.GetCamel(c.ColumnName)}, set{c.ColumnName}] = React.useState(null);");

            outputFile.WriteLine($"");
            outputFile.WriteLine($"    const Swal = useSwalWrapper();");
            outputFile.WriteLine($"    const theme = useTheme();");
            outputFile.WriteLine("    const commandAlert = (variant, keyWord, customText) => {");
            outputFile.WriteLine($"        if (customText == undefined)");
            outputFile.WriteLine("            customText = `Se ${keyWord} el registro con exito!`;");
            outputFile.WriteLine($"");
            outputFile.WriteLine("        const Toast = Swal.mixin({ toast: true, position: 'top-end', showConfirmButton: false, timer: 2800, timerProgressBar: true, onOpen: toast => { toast.addEventListener('mouseenter', Swal.stopTimer); toast.addEventListener('mouseleave', Swal.resumeTimer);}});");
            outputFile.WriteLine("        Toast.fire({ icon: variant, title: customText, background: theme.palette.background.paper});");
            outputFile.WriteLine("    };");
            outputFile.WriteLine($"");
            outputFile.WriteLine("    React.useEffect(() => {");
            outputFile.WriteLine($"        if (!open)");
            outputFile.WriteLine($"            return;");
            outputFile.WriteLine("        if (createMode) {");
            outputFile.WriteLine($"            setDialogTitle('Crear');");
            outputFile.WriteLine($"            setDialogButtonText('Insertar');");

            prefixFk = "";
            countFk = 0;
            foreach (var c in table.Columns.Where(f => f.IsForeignKey && f.ColumnName != "AuditoriaId"))
            {
                if (table.Columns.Count(f => f.TableTarget == c.TableTarget) > 1)
                {
                    countFk++;
                    prefixFk = countFk.ToString();
                }
                outputFile.WriteLine($"            set{c.TableTarget}{prefixFk}(null);");
                prefixFk = "";
            }

            outputFile.WriteLine("        }");
            outputFile.WriteLine("        else {");
            outputFile.WriteLine($"            setDialogTitle('Editar');");
            outputFile.WriteLine($"            setDialogButtonText('Actualizar');");
            foreach (var c in table.Columns.Where(f => !f.IsPrimaryKey && f.ColumnName != "AuditoriaId"))
                outputFile.WriteLine($"            set{c.ColumnName}(refItem.{Helper.GetCamel(c.ColumnName)});");
            outputFile.WriteLine("        }");
            outputFile.WriteLine("    }, [open]);");
            outputFile.WriteLine($"");

            prefixFk = "";
            countFk = 0;
            foreach (var c in table.Columns.Where(f => f.IsForeignKey && f.TableTarget != "Auditoria"))
            {
                if (table.Columns.Count(f => f.TableTarget == c.TableTarget) > 1)
                {
                    countFk++;
                    prefixFk = countFk.ToString();
                }
                outputFile.WriteLine("    React.useEffect(() => {");
                outputFile.WriteLine($"        if (!{Helper.GetCamel(c.TableTarget)}{prefixFk}s || !{Helper.GetCamel(c.ColumnName)})");
                outputFile.WriteLine("        {");
                outputFile.WriteLine($"            set{c.TableTarget}{prefixFk}(null);");
                outputFile.WriteLine("            return;");
                outputFile.WriteLine("        }");
                outputFile.WriteLine(string.Concat("        set", c.TableTarget, prefixFk, "(", Helper.GetCamel(c.TableTarget), prefixFk, "s.find((e) => { return e.", Helper.GetCamel(c.ColumnName), " === ", Helper.GetCamel(c.ColumnName), "}));"));
                outputFile.WriteLine(string.Concat("    }, [", Helper.GetCamel(c.ColumnName), "]);"));
                outputFile.WriteLine($"");
                prefixFk = "";
            }


            outputFile.WriteLine("    React.useEffect(() => {");
            outputFile.WriteLine($"        if (!createItem)");
            outputFile.WriteLine($"            return;");
            outputFile.WriteLine("        const newItem = createMode ? {} : refItem;");

            prefixFk = "";
            countFk = 0;
            foreach (var c in table.Columns.Where(f => !f.IsIdentity && f.ColumnName != "AuditoriaId"))
            {
                var columnNullable = "";
                if (c.IsForeignKey)
                {
                    if (table.Columns.Count(f => f.TableTarget == c.TableTarget) > 1)
                    {
                        countFk++;
                        prefixFk = countFk.ToString();
                    }
                    columnNullable = c.IsNullable ? $"{Helper.GetCamel(c.TableTarget)}{prefixFk} == null ? null : {Helper.GetCamel(c.TableTarget)}{prefixFk}.{Helper.GetCamel(c.ColumnName)};" : $"{Helper.GetCamel(c.TableTarget)}{prefixFk}.{Helper.GetCamel(c.ColumnName)};";
                    prefixFk = "";
                }
                outputFile.WriteLine(string.Concat($"        newItem.{Helper.GetCamel(c.ColumnName)} = ", c.IsForeignKey ? $"{columnNullable}" : $"{Helper.GetCamel(c.ColumnName)};"));
            }

            outputFile.WriteLine($"        newItem.rowsOfPage = ROWS_OF_PAGE;");
            outputFile.WriteLine($"        newItem.pageNumber = currentPage;");
            outputFile.WriteLine("        if (createMode) {");
            outputFile.WriteLine($"            newItem.{Helper.GetCamel(pk.ColumnName)} = 0;");
            outputFile.WriteLine(string.Concat("            axios.post(`${API_URL}", Helper.GetCamel(table.TableName), "/create`, newItem).then((response) => {"));
            outputFile.WriteLine($"                if(!response.data)");
            outputFile.WriteLine($"                     return;");
            outputFile.WriteLine($"                set{table.TableName}s(response.data);");
            outputFile.WriteLine($"                setTotalPages(response.data[0].totalPages);");
            outputFile.WriteLine($"                commandAlert('success', 'creó', null);");
            outputFile.WriteLine("            });");
            outputFile.WriteLine("        }");
            outputFile.WriteLine("        else {");
            outputFile.WriteLine(string.Concat("            axios.post(`${API_URL}", Helper.GetCamel(table.TableName), "/update`, newItem).then((response) => {"));
            outputFile.WriteLine($"                if(!response.data)");
            outputFile.WriteLine($"                     return;");
            outputFile.WriteLine($"                set{table.TableName}s(response.data);");
            outputFile.WriteLine($"                setTotalPages(response.data[0].totalPages);");
            outputFile.WriteLine($"                commandAlert('success', 'actualizó', null);");
            outputFile.WriteLine("            });");
            outputFile.WriteLine("        }");
            outputFile.WriteLine($"        setCreateItem(false);");
            outputFile.WriteLine("    }, [createItem]);");
            outputFile.WriteLine($"");
            outputFile.WriteLine("    React.useEffect(() => {");
            outputFile.WriteLine($"        if (currentPage == undefined || currentPage === 0)");
            outputFile.WriteLine($"            return;");
            outputFile.WriteLine(string.Concat("        axios.post(`${API_URL}", Helper.GetCamel(table.TableName), "/getPaginated`, { pageNumber: currentPage, rowsOfPage: ROWS_OF_PAGE }).then((response) => {"));
            outputFile.WriteLine($"                if(!response.data)");
            outputFile.WriteLine($"                     return;");
            outputFile.WriteLine($"            set{table.TableName}s(response.data);");
            outputFile.WriteLine($"            setTotalPages(response.data[0].totalPages);");

            prefixFk = "";
            countFk = 0;
            foreach (var c in table.Columns.Where(f => f.IsForeignKey && f.ColumnName != "AuditoriaId"))
            {
                if (table.Columns.Count(f => f.TableTarget == c.TableTarget) > 1)
                {
                    countFk++;
                    prefixFk = countFk.ToString();
                }
                outputFile.WriteLine($"            setCall{c.TableTarget}{prefixFk}(true);");
                prefixFk = "";
            }


            outputFile.WriteLine("        });");
            outputFile.WriteLine("    }, [currentPage]);");
            outputFile.WriteLine($"");
            prefixFk = "";
            countFk = 0;
            foreach (var c in table.Columns.Where(f => f.IsForeignKey && f.ColumnName != "AuditoriaId"))
            {
                if (table.Columns.Count(f => f.TableTarget == c.TableTarget) > 1)
                {
                    countFk++;
                    prefixFk = countFk.ToString();
                }
                outputFile.WriteLine("    React.useEffect(() => {");
                outputFile.WriteLine($"        if (!call{c.TableTarget}{prefixFk})");
                outputFile.WriteLine($"            return;");
                outputFile.WriteLine(string.Concat("        axios.post(`${API_URL}", Helper.GetCamel(c.TableTarget), prefixFk, "/getPaginated`, { pageNumber: 1, rowsOfPage: 9999 }).then((response) => {"));
                outputFile.WriteLine($"                if(!response.data)");
                outputFile.WriteLine($"                     return;");
                outputFile.WriteLine($"            set{c.TableTarget}{prefixFk}s(response.data);");
                outputFile.WriteLine("        });");
                outputFile.WriteLine(string.Concat("    }, [call", c.TableTarget, prefixFk, "]);"));
                outputFile.WriteLine($"");
                prefixFk = "";
            }
            outputFile.WriteLine($"    if (!{Helper.GetCamel(table.TableName)}s) return null;");
            outputFile.WriteLine($"    return (");
            outputFile.WriteLine($"        <React.Fragment>");
            outputFile.WriteLine("            <Stack direction={'row'} spacing={3}>");
            outputFile.WriteLine(string.Concat("                <Typography variant={'h2'} mb={3}>", table.Catalog.FormName, "</Typography>"));
            outputFile.WriteLine("                <Fab size='small' onClick={() => { setOpen(true); setCreateMode(true); setRefItem({}); }} color={'primary'}><AddCircleIcon /></Fab>");
            outputFile.WriteLine($"            </Stack>");
            outputFile.WriteLine("            <Stack spacing={2}>");
            outputFile.WriteLine($"                <JumboCardQuick");
            var columnsArray = table.Columns.Where(f => !f.IsIdentity && f.ColumnName != "AuditoriaId").ToList();
            var formLayoutArray = Helper.GetFormLayout(columnsArray.Count).ToArray();

            outputFile.WriteLine("                    title={");
            outputFile.WriteLine("                        <Grid container spacing={1}>");
            //CABECERA GRID
            var gridColumnLayout = Helper.GetColumnLayout(columnsArray.Count);
            var gridColumnSizeArray = gridColumnLayout.Item1.ToArray();
            for (int i = 0; i <= columnsArray.Count - 1; i++)
            {
                outputFile.WriteLine(string.Concat("                            <Grid item xs={", gridColumnSizeArray[i], "} md={", gridColumnSizeArray[i], "} lg={", gridColumnSizeArray[i], "}>"));
                if (!columnsArray[i].IsForeignKey)
                {
                    outputFile.WriteLine(string.Concat($"                                 {columnsArray[i].Property.FormTitle}"));
                }
                else
                {
                    var fkTableInfo = project.Tables.First(f => f.TableName == columnsArray[i].TableTarget);
                    var fkColumnsInfo = fkTableInfo.Columns;
                    var namedColumnInfo = fkColumnsInfo.FirstOrDefault(f => f.ColumnName.ToLower().Contains("nombre") || f.ColumnName.ToLower().Contains("descripcion") && !f.ColumnName.ToLower().Contains("descripcionid") || f.ColumnName.ToLower().Contains("codigo"));
                    namedColumnInfo ??= fkColumnsInfo.First(f => f.IsPrimaryKey);

                    outputFile.WriteLine(string.Concat($"                                 {namedColumnInfo.ColumnName}"));
                }

                outputFile.WriteLine($"                            </Grid>");
            }
            if (gridColumnLayout.Item2)
            {
                outputFile.WriteLine(string.Concat("                            <Grid item xs={", gridColumnLayout.Item3, "} md={", gridColumnLayout.Item3, "} lg={", gridColumnLayout.Item3, "}>"));
                outputFile.WriteLine("                                FechaModificacion");
                outputFile.WriteLine($"                            </Grid>");
                outputFile.WriteLine(string.Concat("                            <Grid item xs={", gridColumnLayout.Item4, "} md={", gridColumnLayout.Item4, "} lg={", gridColumnLayout.Item4, "}>"));
                outputFile.WriteLine("                                Usuario");
                outputFile.WriteLine($"                            </Grid>");
            }
            outputFile.WriteLine($"                        </Grid>");
            outputFile.WriteLine("                    }");

            outputFile.WriteLine("                    headerSx={{ borderBottom: 1, borderBottomColor: 'divider' }} wrapperSx={{ p: 1 }}>");
            outputFile.WriteLine("                    {");
            outputFile.WriteLine(string.Concat("                        ", Helper.GetCamel(table.TableName), "s.map((", Helper.GetCamel(table.TableName), ", key) => (<", table.TableName, "Item item={", Helper.GetCamel(table.TableName), "} setItems={set", table.TableName, "s} key={", Helper.GetCamel(table.TableName), ".Id} currentPage={currentPage} setTotalPages={setTotalPages} commandAlert={commandAlert} setOpen={setOpen} setRefItem={setRefItem} setCreateMode={setCreateMode} />))"));
            outputFile.WriteLine("                    }");
            outputFile.WriteLine($"                </JumboCardQuick>");
            outputFile.WriteLine("                <Pagination color='primary' count={totalPages} page={currentPage} onChange={(event, value) => setCurrentPage(value)} />");
            outputFile.WriteLine($"            </Stack>");
            outputFile.WriteLine($"            <Div>");
            outputFile.WriteLine("                <Dialog fullWidth='true' maxWidth='xl' open={open} onClose={() => setOpen(false)}>");
            outputFile.WriteLine($"                    <DialogContent>");
            outputFile.WriteLine("                        <JumboCardQuick title={dialogTitle}");
            outputFile.WriteLine("                            wrapperSx={{ backgroundColor: 'background.paper', pt: 0 }}>");
            outputFile.WriteLine("                            <Box p={2}>");
            outputFile.WriteLine("                               <form ref={formRef}>");
            int j = 0;
            int count = 0;
            prefixFk = "";
            countFk = 0;
            foreach (var row in formLayoutArray)
            {
                count++;
                outputFile.WriteLine(string.Concat("                                  <Grid container spacing={3} p={1} ", count == formLayoutArray.Length ? "pb={3}" : "", ">"));
                foreach (var c in row)
                {
                    outputFile.WriteLine(string.Concat("                                     <Grid item xs={", c, "} md={", c, "} lg={", c, "} >"));
                    if (!columnsArray[j].IsForeignKey)
                    {
                        outputFile.WriteLine(string.Concat("                                         <TextField fullWidth ", columnsArray[j].IsNullable ? "" : "required ", "id='outlined", columnsArray[j].IsNullable ? "" : "-required", "' label='", columnsArray[j].ColumnName, "' defaultValue={refItem.", Helper.GetCamel(columnsArray[j].ColumnName), "} inputProps={{ maxLength: ", columnsArray[j].SqlDataType == "varchar" ? columnsArray[j].MaxLength : columnsArray[j].Precision, " }} onChange={(event) => { set", columnsArray[j].ColumnName, "(event.target.value);}} />"));
                    }
                    else
                    {
                        outputFile.WriteLine(string.Concat("                                         <Autocomplete"));
                        if (!columnsArray[j].IsNullable)
                            outputFile.WriteLine(string.Concat("                                             required"));

                        outputFile.WriteLine(string.Concat("                                             disablePortal"));
                        outputFile.WriteLine(string.Concat("                                             id=\"combo-box-demo\""));
                        var fkTableInfo = project.Tables.First(f => f.TableName == columnsArray[j].TableTarget);
                        var fkColumnsInfo = fkTableInfo.Columns;
                        var namedColumnInfo = fkColumnsInfo.FirstOrDefault(f => f.ColumnName.ToLower().Contains("nombre") || f.ColumnName.ToLower().Contains("descripcion") && !f.ColumnName.ToLower().Contains("descripcionid") || f.ColumnName.ToLower().Contains("codigo"));
                        namedColumnInfo ??= fkColumnsInfo.First(f => f.IsPrimaryKey);

                        if (table.Columns.Where(f => f.TableTarget == columnsArray[j].TableTarget).Count() > 1)
                        {
                            countFk++;
                            prefixFk = countFk.ToString();
                        }

                        outputFile.WriteLine(string.Concat("                                             getOptionLabel={(o) => o.", Helper.GetCamel(namedColumnInfo.ColumnName), "}"));
                        outputFile.WriteLine(string.Concat("                                             options={", Helper.GetCamel(fkTableInfo.TableName), prefixFk, "s}"));
                        outputFile.WriteLine(string.Concat("                                             value={", Helper.GetCamel(fkTableInfo.TableName), prefixFk, "}"));
                        outputFile.WriteLine(string.Concat("                                             onChange={(event, newValue) => {"));
                        outputFile.WriteLine(string.Concat("                                                 set", fkTableInfo.TableName, prefixFk, "(newValue);"));
                        outputFile.WriteLine(string.Concat("                                             }}"));
                        outputFile.WriteLine(string.Concat("                                             renderInput={(params) => <TextField {...params} label=\"", fkTableInfo.TableName, prefixFk, "\" ", columnsArray[j].IsNullable ? "" : "required", " />}"));
                        outputFile.WriteLine(string.Concat("                                         />"));
                        prefixFk = "";
                    }
                    outputFile.WriteLine($"                                      </Grid>");
                    j++;
                }
                outputFile.WriteLine($"                                  </Grid>");
            }
            outputFile.WriteLine("                                   <Button variant='contained' endIcon={<SendIcon />} onClick={() => { ");
            outputFile.WriteLine("                                       if (!formRef.current.reportValidity())");
            outputFile.WriteLine("                                           return;");
            outputFile.WriteLine("                                       setCreateItem(true); ");
            outputFile.WriteLine("                                       setOpen(false); }} >");
            outputFile.WriteLine("                                       {dialogButtonText}");
            outputFile.WriteLine($"                                  </Button>");
            outputFile.WriteLine($"                              </form>");
            outputFile.WriteLine($"                          </Box>");

            outputFile.WriteLine($"                        </JumboCardQuick>");
            outputFile.WriteLine($"                    </DialogContent>");
            outputFile.WriteLine($"                    <DialogActions>");
            outputFile.WriteLine("                        <Button onClick={() => { setOpen(false); }}>Cerrar</Button>");
            outputFile.WriteLine($"                    </DialogActions>");
            outputFile.WriteLine($"                </Dialog>");
            outputFile.WriteLine($"            </Div>");
            outputFile.WriteLine($"        </React.Fragment>");
            outputFile.WriteLine($"    );");
            outputFile.WriteLine("};");
            outputFile.WriteLine($"export default {table.TableName};");
            outputFile.Close();
            outputFile.Dispose();
        }

        public static void WriteComponentItems(Project project, Table table)
        {
            if (!Directory.Exists(Path.Combine(project.UIComponentsPath)))
                Directory.CreateDirectory(Path.Combine(project.UIComponentsPath));
            using StreamWriter outputFile = new(Path.Combine(project.UIComponentsPath, string.Concat(table.TableName, "Item.js")), false, Encoding.UTF8);
            var pk = table.Columns.First(f => f.IsPrimaryKey);
            outputFile.WriteLine($"import React from 'react';");
            outputFile.WriteLine($"import axios from 'axios';");
            outputFile.WriteLine($"import EditIcon from '@mui/icons-material/Edit';");
            outputFile.WriteLine($"import DeleteIcon from '@mui/icons-material/Delete';");
            outputFile.WriteLine("import {API_URL,ROWS_OF_PAGE} from '../../utils/constants/paths';");
            outputFile.WriteLine("import {Card,ListItemIcon, ListItemButton, Fab,Grid} from '@mui/material';");
            outputFile.WriteLine($"import useSwalWrapper from '@jumbo/vendors/sweetalert2/hooks';");
            outputFile.WriteLine($"");
            outputFile.WriteLine(string.Concat("const ", table.TableName, "Item = ({item,setItems, currentPage, setTotalPages, commandAlert, setOpen, setRefItem, setCreateMode }) => {"));
            outputFile.WriteLine($"    const [deleteItem, setDeleteItem] = React.useState(false);");
            outputFile.WriteLine($"    const Swal = useSwalWrapper();");
            outputFile.WriteLine("    const confirmDelete = (keyItem) => { Swal.fire({ title: `¿Está seguro de eliminar: ${keyItem}?`, text: 'No se podrá revertir esta acción!', icon: 'warning', showCancelButton: true, confirmButtonText: 'Si, eliminar!', cancelButtonText: 'No, cancelar!', reverseButtons: true }).then(result => {if (result.value) {setDeleteItem(true);} else if (result.dismiss === Swal.DismissReason.cancel) {}});};");
            outputFile.WriteLine("    React.useEffect(() => {");
            outputFile.WriteLine($"        if (!deleteItem)");
            outputFile.WriteLine($"            return;");
            outputFile.WriteLine($"        let toDeleteItem = item;");
            outputFile.WriteLine($"        toDeleteItem.rowsOfPage = ROWS_OF_PAGE;");
            outputFile.WriteLine($"        toDeleteItem.pageNumber = currentPage;");
            outputFile.WriteLine(string.Concat("        axios.post(`${API_URL}", Helper.GetCamel(table.TableName), "/delete`, toDeleteItem).then((response) => {"));
            outputFile.WriteLine($"                if(!response.data)");
            outputFile.WriteLine($"                     return;");
            outputFile.WriteLine($"            setItems(response.data);");
            outputFile.WriteLine($"            setTotalPages(response.data[0].totalPages);");
            outputFile.WriteLine($"            commandAlert('success','eliminó',null);");
            outputFile.WriteLine($"            setDeleteItem(false);");
            outputFile.WriteLine("        }).catch((error) => {");
            outputFile.WriteLine("            if (error.response) {");
            outputFile.WriteLine($"                commandAlert('error','',error.response.data);");
            outputFile.WriteLine("            }   ");
            outputFile.WriteLine("            setDeleteItem(false);");
            outputFile.WriteLine("        });");
            outputFile.WriteLine("    }, [deleteItem]);");
            outputFile.WriteLine($"");
            outputFile.WriteLine($"    return (");
            outputFile.WriteLine("        <Card sx={{ mb: 0.5 }}>");
            outputFile.WriteLine("            <ListItemButton component={'li'} sx={{p: theme => theme.spacing(1, 3), '&:hover .ListItemIcons': { opacity: 1}}}>");

            outputFile.WriteLine("                <Grid container alignItems='center' justifyContent='center' spacing={3.75} sx={{ p: theme => theme.spacing(0.8, 1) }} >");
            //DETALLE GRID
            var columnsArray = table.Columns.Where(f => !f.IsIdentity && f.ColumnName != "AuditoriaId").ToList();
            var gridColumnLayout = Helper.GetColumnLayout(columnsArray.Count);
            var gridColumnSizeArray = gridColumnLayout.Item1.ToArray();
            for (int i = 0; i <= columnsArray.Count - 1; i++)
            {
                outputFile.WriteLine(string.Concat("                    <Grid item xs={", gridColumnSizeArray[i], "} md={", gridColumnSizeArray[i], "} lg={", gridColumnSizeArray[i], "}>"));
                if (!columnsArray[i].IsForeignKey)
                {
                    outputFile.WriteLine(string.Concat("                        {item.", Helper.GetCamel(columnsArray[i].ColumnName), "}"));
                }
                else
                {
                    var fkTableInfo = project.Tables.First(f => f.TableName == columnsArray[i].TableTarget);
                    var fkColumnsInfo = fkTableInfo.Columns;
                    var namedColumnInfo = fkColumnsInfo.FirstOrDefault(f => f.ColumnName.ToLower().Contains("nombre") || f.ColumnName.ToLower().Contains("descripcion") && !f.ColumnName.ToLower().Contains("descripcionid") || f.ColumnName.ToLower().Contains("codigo"));
                    namedColumnInfo ??= fkColumnsInfo.First(f => f.IsPrimaryKey);
                    outputFile.WriteLine(string.Concat("                        {item.", Helper.GetCamel(fkTableInfo.TableName), namedColumnInfo.ColumnName, "}"));
                }
                outputFile.WriteLine($"                    </Grid>");
            }

            if (gridColumnLayout.Item2)
            {
                outputFile.WriteLine(string.Concat("                    <Grid item xs={", gridColumnLayout.Item3, "} md={", gridColumnLayout.Item3, "} lg={", gridColumnLayout.Item3, "}>"));
                outputFile.WriteLine("                        {item.fechaModificacion}");
                outputFile.WriteLine($"                    </Grid>");
                outputFile.WriteLine(string.Concat("                    <Grid item xs={", gridColumnLayout.Item4, "} md={", gridColumnLayout.Item4, "} lg={", gridColumnLayout.Item4, "}>"));
                outputFile.WriteLine("                        {item.nombreCortoUsuario}");
                outputFile.WriteLine($"                    </Grid>");
            }
            outputFile.WriteLine($"                </Grid>");

            outputFile.WriteLine("                <ListItemIcon className={'ListItemIcons'} sx={{position: 'absolute', right: 24, top: 7, transition: 'all 0.2s', opacity: 0 }}>");
            outputFile.WriteLine("                    <Fab onClick={() => { setOpen(true); setRefItem(item); setCreateMode(false); }} size='small' color={'primary'} sx={{ right: 14 }}><EditIcon /></Fab>");
            string relevantColumnName = pk.ColumnName;
            var nameDescriptionColumn = table.Columns.FirstOrDefault(f => f.ColumnName.ToLower().Contains("nombre") || f.ColumnName.ToLower().Contains("descripcion"));
            if (nameDescriptionColumn != null)
                relevantColumnName = nameDescriptionColumn.ColumnName;
            outputFile.WriteLine(string.Concat("                    <Fab onClick={() => confirmDelete(item.", Helper.GetCamel(relevantColumnName), ")} size='small' color={'secondary'}><DeleteIcon /></Fab>"));
            outputFile.WriteLine($"                </ListItemIcon>");
            outputFile.WriteLine($"            </ListItemButton>");
            outputFile.WriteLine($"        </Card>");
            outputFile.WriteLine($"    );");
            outputFile.WriteLine("};");
            outputFile.WriteLine($"export default {table.TableName}Item;");
            outputFile.Close();
            outputFile.Dispose();
        }
    }
}
