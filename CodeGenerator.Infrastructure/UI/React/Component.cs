using CodeGenerator.Infrastructure.Context.Models;
using System.Text;

namespace CodeGenerator.Infrastructure.UI.React
{
    public static class Component
    {
        public static async Task WriteComponents(Project project, Table table)
        {
            if (!Directory.Exists(Path.Combine(project.UIComponentsPath)))
                Directory.CreateDirectory(Path.Combine(project.UIComponentsPath));
            using StreamWriter outputFile = new(Path.Combine(project.UIComponentsPath, string.Concat(table.TableName, ".js")), false, Encoding.UTF8);
            var pk = table.Columns.First(f => f.IsPrimaryKey);
            await outputFile.WriteLineAsync($"import React from 'react';");
            await outputFile.WriteLineAsync($"import axios from 'axios';");
            await outputFile.WriteLineAsync($"import {table.TableName}Item from '../../pages/catalogos/{table.TableName}Item';");
            await outputFile.WriteLineAsync($"import SendIcon from '@mui/icons-material/Send';");
            await outputFile.WriteLineAsync("import {Typography,Pagination, Grid, Stack, Fab, Button, Dialog, DialogActions, DialogContent, TextField, useTheme, Box } from '@mui/material';");
            await outputFile.WriteLineAsync("import Autocomplete from '@mui/material/Autocomplete';");
            await outputFile.WriteLineAsync($"import AddCircleIcon from '@mui/icons-material/AddCircle';");
            await outputFile.WriteLineAsync("import {useTranslation} from 'react-i18next';");
            await outputFile.WriteLineAsync("import {API_URL,ROWS_OF_PAGE} from '../../utils/constants/paths';");
            await outputFile.WriteLineAsync($"import JumboCardQuick from '@jumbo/components/JumboCardQuick';");
            await outputFile.WriteLineAsync($"import Div from '@jumbo/shared/Div';");
            await outputFile.WriteLineAsync($"import useSwalWrapper from '@jumbo/vendors/sweetalert2/hooks';");
            await outputFile.WriteLineAsync($"");
            await outputFile.WriteLineAsync(string.Concat("const ", table.TableName, " = () => {"));
            await outputFile.WriteLineAsync("    const { t } = useTranslation();");
            await outputFile.WriteLineAsync("    const formRef = React.useRef();");
            await outputFile.WriteLineAsync($"    const [{Helper.GetCamel(table.TableName)}s, set{table.TableName}s] = React.useState(null);");
            foreach (var c in table.Columns.Where(f => f.IsForeignKey && f.ColumnName != "AuditId"))
            {
                
                await outputFile.WriteLineAsync($"    const [{Helper.GetCamel(c.TableTarget)}s, set{c.TableTarget}s] = React.useState(false);");
                await outputFile.WriteLineAsync(string.Concat("    const [", Helper.GetCamel(c.TableTarget), ", set", c.TableTarget, "] = React.useState({ });"));
                await outputFile.WriteLineAsync($"    const [call{c.TableTarget}, setCall{c.TableTarget}] = React.useState(false);");
            }
        
            await outputFile.WriteLineAsync($"    const [totalPages, setTotalPages] = React.useState(0);");
            await outputFile.WriteLineAsync($"    const [currentPage, setCurrentPage] = React.useState(1);");
            await outputFile.WriteLineAsync($"    const [open, setOpen] = React.useState(false);");
            await outputFile.WriteLineAsync($"    const [createMode, setCreateMode] = React.useState(true);");
            await outputFile.WriteLineAsync($"    const [createItem, setCreateItem] = React.useState(false);");
            await outputFile.WriteLineAsync($"    const [dialogTitle, setDialogTitle] = React.useState('');");
            await outputFile.WriteLineAsync($"    const [dialogButtonText, setDialogButtonText] = React.useState('');");
            await outputFile.WriteLineAsync("    const [refItem, setRefItem] = React.useState({});");
            foreach (var c in table.Columns.Where(f => !f.IsIdentity && f.ColumnName != "AuditId"))
                await outputFile.WriteLineAsync($"    const [{Helper.GetCamel(c.ColumnName)}, set{c.ColumnName}] = React.useState(null);");
        
            await outputFile.WriteLineAsync($"");
            await outputFile.WriteLineAsync($"    const Swal = useSwalWrapper();");
            await outputFile.WriteLineAsync($"    const theme = useTheme();");
            await outputFile.WriteLineAsync("    const commandAlert = (variant, keyWord, customText) => {");
            await outputFile.WriteLineAsync($"        if (customText == undefined)");
            await outputFile.WriteLineAsync("            customText = `Se ${keyWord} el registro con exito!`;");
            await outputFile.WriteLineAsync($"");
            await outputFile.WriteLineAsync("        const Toast = Swal.mixin({ toast: true, position: 'top-end', showConfirmButton: false, timer: 2800, timerProgressBar: true, onOpen: toast => { toast.addEventListener('mouseenter', Swal.stopTimer); toast.addEventListener('mouseleave', Swal.resumeTimer);}});");
            await outputFile.WriteLineAsync("        Toast.fire({ icon: variant, title: customText, background: theme.palette.background.paper});");
            await outputFile.WriteLineAsync("    };");
            await outputFile.WriteLineAsync($"");
            await outputFile.WriteLineAsync("    React.useEffect(() => {");
            await outputFile.WriteLineAsync($"        if (!open)");
            await outputFile.WriteLineAsync($"            return;");
            await outputFile.WriteLineAsync("        if (createMode) {");
            await outputFile.WriteLineAsync($"            setDialogTitle('Crear');");
            await outputFile.WriteLineAsync($"            setDialogButtonText('Insertar');");
            foreach (var c in table.Columns.Where(f => f.IsForeignKey && f.ColumnName != "AuditId"))
                await outputFile.WriteLineAsync($"            set{c.TableTarget}(null);");
            await outputFile.WriteLineAsync("        }");
            await outputFile.WriteLineAsync("        else {");
            await outputFile.WriteLineAsync($"            setDialogTitle('Editar');");
            await outputFile.WriteLineAsync($"            setDialogButtonText('Actualizar');");
            foreach (var c in table.Columns.Where(f => !f.IsPrimaryKey && f.ColumnName != "AuditId"))
                await outputFile.WriteLineAsync($"            set{c.ColumnName}(refItem.{Helper.GetCamel(c.ColumnName)});");
            await outputFile.WriteLineAsync("        }");
            await outputFile.WriteLineAsync("    }, [open]);");
            await outputFile.WriteLineAsync($"");
            foreach (var c in table.Columns.Where(f => f.IsForeignKey && f.TableName != "AuditId"))
            {
                await outputFile.WriteLineAsync("    React.useEffect(() => {");
                await outputFile.WriteLineAsync($"        if (!{Helper.GetCamel(c.TableTarget)}s || !{Helper.GetCamel(c.ColumnName)})");
                await outputFile.WriteLineAsync("        {");
                await outputFile.WriteLineAsync($"            set{c.TableTarget}(null);");
                await outputFile.WriteLineAsync("            return;");
                await outputFile.WriteLineAsync("        }");
                await outputFile.WriteLineAsync(string.Concat("        set", c.TableTarget, "(", Helper.GetCamel(c.TableTarget), "s.find((e) => { return e.", Helper.GetCamel(c.ColumnName), " === ", Helper.GetCamel(c.ColumnName), "}));"));
                await outputFile.WriteLineAsync(string.Concat("    }, [", Helper.GetCamel(c.ColumnName), "]);"));
                await outputFile.WriteLineAsync($"");
            }
        
        
            await outputFile.WriteLineAsync("    React.useEffect(() => {");
            await outputFile.WriteLineAsync($"        if (!createItem)");
            await outputFile.WriteLineAsync($"            return;");
            await outputFile.WriteLineAsync("        const newItem = createMode ? {} : refItem;");
            foreach (var c in table.Columns.Where(f => !f.IsIdentity && f.ColumnName != "AuditId"))
            {
                var columnNullable = "";
                if (c.IsForeignKey)
                    columnNullable = c.IsNullable ? $"{Helper.GetCamel(c.TableTarget)} == null ? null : {Helper.GetCamel(c.TableTarget)}.{Helper.GetCamel(c.ColumnName)};" : $"{Helper.GetCamel(c.TableTarget)}.{Helper.GetCamel(c.ColumnName)};";
                await outputFile.WriteLineAsync(string.Concat($"        newItem.{Helper.GetCamel(c.ColumnName)} = ", c.IsForeignKey ? $"{columnNullable}" : $"{Helper.GetCamel(c.ColumnName)};"));
            }
        
            await outputFile.WriteLineAsync($"        newItem.rowsOfPage = ROWS_OF_PAGE;");
            await outputFile.WriteLineAsync($"        newItem.pageNumber = currentPage;");
            await outputFile.WriteLineAsync("        if (createMode) {");
            await outputFile.WriteLineAsync($"            newItem.{Helper.GetCamel(pk.ColumnName)} = 0;");
            await outputFile.WriteLineAsync(string.Concat("            axios.post(`${API_URL}", Helper.GetCamel(table.TableName), "/create`, newItem).then((response) => {"));
            await outputFile.WriteLineAsync($"                set{table.TableName}s(response.data);");
            await outputFile.WriteLineAsync($"                setTotalPages(response.data[0].totalPages);");
            await outputFile.WriteLineAsync($"                commandAlert('success', 'creó', null);");
            await outputFile.WriteLineAsync("            });");
            await outputFile.WriteLineAsync("        }");
            await outputFile.WriteLineAsync("        else {");
            await outputFile.WriteLineAsync(string.Concat("            axios.post(`${API_URL}", Helper.GetCamel(table.TableName), "/update`, newItem).then((response) => {"));
            await outputFile.WriteLineAsync($"                set{table.TableName}s(response.data);");
            await outputFile.WriteLineAsync($"                setTotalPages(response.data[0].totalPages);");
            await outputFile.WriteLineAsync($"                commandAlert('success', 'actualizó', null);");
            await outputFile.WriteLineAsync("            });");
            await outputFile.WriteLineAsync("        }");
            await outputFile.WriteLineAsync($"        setCreateItem(false);");
            await outputFile.WriteLineAsync("    }, [createItem]);");
            await outputFile.WriteLineAsync($"");
            await outputFile.WriteLineAsync("    React.useEffect(() => {");
            await outputFile.WriteLineAsync($"        if (currentPage == undefined || currentPage === 0)");
            await outputFile.WriteLineAsync($"            return;");
            await outputFile.WriteLineAsync(string.Concat("        axios.post(`${API_URL}", Helper.GetCamel(table.TableName), "/getPaginated`, { pageNumber: currentPage, rowsOfPage: ROWS_OF_PAGE }).then((response) => {"));
            await outputFile.WriteLineAsync($"            set{table.TableName}s(response.data);");
            await outputFile.WriteLineAsync($"            setTotalPages(response.data[0].totalPages);");
            foreach (var c in table.Columns.Where(f => f.IsForeignKey && f.ColumnName != "AuditId"))
                await outputFile.WriteLineAsync($"            setCall{c.TableTarget}(true);");
        
            await outputFile.WriteLineAsync("        });");
            await outputFile.WriteLineAsync("    }, [currentPage]);");
            await outputFile.WriteLineAsync($"");
            foreach (var c in table.Columns.Where(f => f.IsForeignKey && f.ColumnName != "AuditId"))
            {
                await outputFile.WriteLineAsync("    React.useEffect(() => {");
                await outputFile.WriteLineAsync($"        if (!call{c.TableTarget})");
                await outputFile.WriteLineAsync($"            return;");
                await outputFile.WriteLineAsync(string.Concat("        axios.post(`${API_URL}", Helper.GetCamel(c.TableTarget), "/getPaginated`, { pageNumber: 1, rowsOfPage: 9999 }).then((response) => {"));
                await outputFile.WriteLineAsync($"            set{c.TableTarget}s(response.data);");
                await outputFile.WriteLineAsync("        });");
                await outputFile.WriteLineAsync(string.Concat("    }, [call", c.TableTarget, "]);"));
                await outputFile.WriteLineAsync($"");
            }
            await outputFile.WriteLineAsync($"    if (!{Helper.GetCamel(table.TableName)}s) return null;");
            await outputFile.WriteLineAsync($"    return (");
            await outputFile.WriteLineAsync($"        <React.Fragment>");
            await outputFile.WriteLineAsync("            <Stack direction={'row'} spacing={3}>");
            await outputFile.WriteLineAsync(string.Concat("                <Typography variant={'h2'} mb={3}>", table.Catalog.FormName, "</Typography>"));
            await outputFile.WriteLineAsync("                <Fab size='small' onClick={() => { setOpen(true); setCreateMode(true); setRefItem({}); }} color={'primary'}><AddCircleIcon /></Fab>");
            await outputFile.WriteLineAsync($"            </Stack>");
            await outputFile.WriteLineAsync("            <Stack spacing={2}>");
            await outputFile.WriteLineAsync($"                <JumboCardQuick");
            var columnsArray = table.Columns.Where(f => !f.IsIdentity && f.ColumnName != "AuditId").ToList();
            var formLayoutArray = Helper.GetFormLayout(columnsArray.Count).ToArray();
            
            await outputFile.WriteLineAsync("                    title={");
            await outputFile.WriteLineAsync("                        <Grid container spacing={1}>");
            //CABECERA GRID
            var gridColumnLayout = Helper.GetColumnLayout(columnsArray.Count);
            var gridColumnSizeArray = gridColumnLayout.Item1.ToArray();
            for (int i = 0; i <= columnsArray.Count - 1; i++)
            {
                await outputFile.WriteLineAsync(string.Concat("                            <Grid item xs={", gridColumnSizeArray[i], "} md={", gridColumnSizeArray[i], "} lg={", gridColumnSizeArray[i], "}>"));
                if (!columnsArray[i].IsForeignKey)
                {
                    await outputFile.WriteLineAsync(string.Concat($"                                 {columnsArray[i].TableName}"));
                }
                else
                {
                    var fkTableInfo = project.Tables.First(f => f.TableName == columnsArray[i].TableTarget);
                    var fkColumnsInfo = fkTableInfo.Columns;
                    var namedColumnInfo = fkColumnsInfo.FirstOrDefault(f => f.ColumnName.ToLower().Contains("nombre") || (f.ColumnName.ToLower().Contains("descripcion") && !f.ColumnName.ToLower().Contains("descripcionid")) || f.ColumnName.ToLower().Contains("codigo"));
                    namedColumnInfo ??= fkColumnsInfo.First(f => f.IsPrimaryKey);
        
                    await outputFile.WriteLineAsync(string.Concat($"                                 {namedColumnInfo.ColumnName}"));
                }
        
                await outputFile.WriteLineAsync($"                            </Grid>");
            }
            if (gridColumnLayout.Item2)
            {
                await outputFile.WriteLineAsync(string.Concat("                            <Grid item xs={", gridColumnLayout.Item3, "} md={", gridColumnLayout.Item3, "} lg={", gridColumnLayout.Item3, "}>"));
                await outputFile.WriteLineAsync("                                FechaModificacion");
                await outputFile.WriteLineAsync($"                            </Grid>");
                await outputFile.WriteLineAsync(string.Concat("                            <Grid item xs={", gridColumnLayout.Item4, "} md={", gridColumnLayout.Item4, "} lg={", gridColumnLayout.Item4, "}>"));
                await outputFile.WriteLineAsync("                                Usuario");
                await outputFile.WriteLineAsync($"                            </Grid>");
            }
            await outputFile.WriteLineAsync($"                        </Grid>");
            await outputFile.WriteLineAsync("                    }");
            
            await outputFile.WriteLineAsync("                    headerSx={{ borderBottom: 1, borderBottomColor: 'divider' }} wrapperSx={{ p: 1 }}>");
            await outputFile.WriteLineAsync("                    {");
            await outputFile.WriteLineAsync(string.Concat("                        ", Helper.GetCamel(table.TableName), "s.map((", Helper.GetCamel(table.TableName), ", key) => (<", table.TableName, "Item item={", Helper.GetCamel(table.TableName), "} setItems={set", table.TableName, "s} key={", Helper.GetCamel(table.TableName), ".Id} currentPage={currentPage} setTotalPages={setTotalPages} commandAlert={commandAlert} setOpen={setOpen} setRefItem={setRefItem} setCreateMode={setCreateMode} />))"));
            await outputFile.WriteLineAsync("                    }");
            await outputFile.WriteLineAsync($"                </JumboCardQuick>");
            await outputFile.WriteLineAsync("                <Pagination color='primary' count={totalPages} page={currentPage} onChange={(event, value) => setCurrentPage(value)} />");
            await outputFile.WriteLineAsync($"            </Stack>");
            await outputFile.WriteLineAsync($"            <Div>");
            await outputFile.WriteLineAsync("                <Dialog fullWidth='true' maxWidth='xl' open={open} onClose={() => setOpen(false)}>");
            await outputFile.WriteLineAsync($"                    <DialogContent>");
            await outputFile.WriteLineAsync("                        <JumboCardQuick title={dialogTitle}");
            await outputFile.WriteLineAsync("                            wrapperSx={{ backgroundColor: 'background.paper', pt: 0 }}>");
            await outputFile.WriteLineAsync("                            <Box p={2}>");
            await outputFile.WriteLineAsync("                               <form ref={formRef}>");
            int j = 0;
            int count = 0;
            foreach (var row in formLayoutArray)
            {
                count++;
                await outputFile.WriteLineAsync(string.Concat("                                  <Grid container spacing={3} p={1} ", (count == formLayoutArray.Length ? "pb={3}" : ""), ">"));
                foreach (var c in row)
                {
                    await outputFile.WriteLineAsync(string.Concat("                                     <Grid item xs={", c, "} md={", c, "} lg={", c, "} >"));
                    if (!columnsArray[j].IsForeignKey)
                    {
                        await outputFile.WriteLineAsync(string.Concat("                                         <TextField fullWidth ", columnsArray[j].IsNullable ? "" : "required ", "id='outlined", columnsArray[j].IsNullable ? "" : "-required", "' label='", columnsArray[j].ColumnName, "' defaultValue={refItem.", Helper.GetCamel(columnsArray[j].ColumnName), "} inputProps={{ maxLength: ", columnsArray[j].SqlDataType == "varchar" ? columnsArray[j].MaxLength : columnsArray[j].Precision, " }} onChange={(event) => { set", columnsArray[j].ColumnName, "(event.target.value);}} />"));
                    }
                    else
                    {
                        await outputFile.WriteLineAsync(string.Concat("                                         <Autocomplete"));
                        if (!columnsArray[j].IsNullable)
                            await outputFile.WriteLineAsync(string.Concat("                                             required"));
        
                        await outputFile.WriteLineAsync(string.Concat("                                             disablePortal"));
                        await outputFile.WriteLineAsync(string.Concat("                                             id=\"combo-box-demo\""));
                        var fkTableInfo = project.Tables.First(f => f.TableName == columnsArray[j].TableTarget);
                        var fkColumnsInfo = fkTableInfo.Columns;
                        var namedColumnInfo = fkColumnsInfo.FirstOrDefault(f => f.ColumnName.ToLower().Contains("nombre") || (f.ColumnName.ToLower().Contains("descripcion") && !f.ColumnName.ToLower().Contains("descripcionid")) || f.ColumnName.ToLower().Contains("codigo"));
                        namedColumnInfo ??= fkColumnsInfo.First(f => f.IsPrimaryKey);
                        await outputFile.WriteLineAsync(string.Concat("                                             getOptionLabel={(o) => o.", Helper.GetCamel(namedColumnInfo.ColumnName), "}"));
                        await outputFile.WriteLineAsync(string.Concat("                                             options={", Helper.GetCamel(fkTableInfo.TableName), "s}"));
                        await outputFile.WriteLineAsync(string.Concat("                                             value={", Helper.GetCamel(fkTableInfo.TableName), "}"));
                        await outputFile.WriteLineAsync(string.Concat("                                             onChange={(event, newValue) => {"));
                        await outputFile.WriteLineAsync(string.Concat("                                                 set", fkTableInfo.TableName, "(newValue);"));
                        await outputFile.WriteLineAsync(string.Concat("                                             }}"));
                        await outputFile.WriteLineAsync(string.Concat("                                             renderInput={(params) => <TextField {...params} label=\"", fkTableInfo.TableName, "\" ", columnsArray[j].IsNullable ? "" : "required", " />}"));
                        await outputFile.WriteLineAsync(string.Concat("                                         />"));
                    }
                    await outputFile.WriteLineAsync($"                                      </Grid>");
                    j++;
                }
                await outputFile.WriteLineAsync($"                                  </Grid>");
            }
            await outputFile.WriteLineAsync("                                   <Button variant='contained' endIcon={<SendIcon />} onClick={() => { ");
            await outputFile.WriteLineAsync("                                       if (!formRef.current.reportValidity())");
            await outputFile.WriteLineAsync("                                           return;");
            await outputFile.WriteLineAsync("                                       setCreateItem(true); ");
            await outputFile.WriteLineAsync("                                       setOpen(false); }} >");
            await outputFile.WriteLineAsync("                                       {dialogButtonText}");
            await outputFile.WriteLineAsync($"                                  </Button>");
            await outputFile.WriteLineAsync($"                              </form>");
            await outputFile.WriteLineAsync($"                          </Box>");
        
            await outputFile.WriteLineAsync($"                        </JumboCardQuick>");
            await outputFile.WriteLineAsync($"                    </DialogContent>");
            await outputFile.WriteLineAsync($"                    <DialogActions>");
            await outputFile.WriteLineAsync("                        <Button onClick={() => { setOpen(false); }}>Cerrar</Button>");
            await outputFile.WriteLineAsync($"                    </DialogActions>");
            await outputFile.WriteLineAsync($"                </Dialog>");
            await outputFile.WriteLineAsync($"            </Div>");
            await outputFile.WriteLineAsync($"        </React.Fragment>");
            await outputFile.WriteLineAsync($"    );");
            await outputFile.WriteLineAsync("};");
            await outputFile.WriteLineAsync($"export default {table.TableName};");
        
        
            return;
        }
        
        public static async Task WriteComponentItems(Project project, Table table)
        {
            if (!Directory.Exists(Path.Combine(project.UIComponentsPath)))
                Directory.CreateDirectory(Path.Combine(project.UIComponentsPath));
            using StreamWriter outputFile = new(Path.Combine(project.UIComponentsPath, string.Concat(table.TableName, "Item.js")), false, Encoding.UTF8);
            var pk = table.Columns.First(f => f.IsPrimaryKey);
            await outputFile.WriteLineAsync($"import React from 'react';");
            await outputFile.WriteLineAsync($"import axios from 'axios';");
            await outputFile.WriteLineAsync($"import EditIcon from '@mui/icons-material/Edit';");
            await outputFile.WriteLineAsync($"import DeleteIcon from '@mui/icons-material/Delete';");
            await outputFile.WriteLineAsync("import {API_URL,ROWS_OF_PAGE} from '../../utils/constants/paths';");
            await outputFile.WriteLineAsync("import {Card,ListItemIcon, ListItemButton, Fab,Grid} from '@mui/material';");
            await outputFile.WriteLineAsync($"import useSwalWrapper from '@jumbo/vendors/sweetalert2/hooks';");
            await outputFile.WriteLineAsync($"");
            await outputFile.WriteLineAsync(string.Concat("const ", table.TableName, "Item = ({item,setItems, currentPage, setTotalPages, commandAlert, setOpen, setRefItem, setCreateMode }) => {"));
            await outputFile.WriteLineAsync($"    const [deleteItem, setDeleteItem] = React.useState(false);");
            await outputFile.WriteLineAsync($"    const Swal = useSwalWrapper();");
            await outputFile.WriteLineAsync("    const confirmDelete = (keyItem) => { Swal.fire({ title: `¿Está seguro de eliminar: ${keyItem}?`, text: 'No se podrá revertir esta acción!', icon: 'warning', showCancelButton: true, confirmButtonText: 'Si, eliminar!', cancelButtonText: 'No, cancelar!', reverseButtons: true }).then(result => {if (result.value) {setDeleteItem(true);} else if (result.dismiss === Swal.DismissReason.cancel) {}});};");
            await outputFile.WriteLineAsync("    React.useEffect(() => {");
            await outputFile.WriteLineAsync($"        if (!deleteItem)");
            await outputFile.WriteLineAsync($"            return;");
            await outputFile.WriteLineAsync($"        let toDeleteItem = item;");
            await outputFile.WriteLineAsync($"        toDeleteItem.rowsOfPage = ROWS_OF_PAGE;");
            await outputFile.WriteLineAsync($"        toDeleteItem.pageNumber = currentPage;");
            await outputFile.WriteLineAsync(string.Concat("        axios.post(`${API_URL}", Helper.GetCamel(table.TableName), "/delete`, toDeleteItem).then((response) => {"));
            await outputFile.WriteLineAsync($"            setItems(response.data);");
            await outputFile.WriteLineAsync($"            setTotalPages(response.data[0].totalPages);");
            await outputFile.WriteLineAsync($"            commandAlert('success','eliminó',null);");
            await outputFile.WriteLineAsync($"            setDeleteItem(false);");
            await outputFile.WriteLineAsync("        }).catch((error) => {");
            await outputFile.WriteLineAsync("            if (error.response) {");
            await outputFile.WriteLineAsync($"                commandAlert('error','',error.response.data);");
            await outputFile.WriteLineAsync("            }   ");
            await outputFile.WriteLineAsync("            setDeleteItem(false);");
            await outputFile.WriteLineAsync("        });");
            await outputFile.WriteLineAsync("    }, [deleteItem]);");
            await outputFile.WriteLineAsync($"");
            await outputFile.WriteLineAsync($"    return (");
            await outputFile.WriteLineAsync("        <Card sx={{ mb: 0.5 }}>");
            await outputFile.WriteLineAsync("            <ListItemButton component={'li'} sx={{p: theme => theme.spacing(1, 3), '&:hover .ListItemIcons': { opacity: 1}}}>");
                    
            await outputFile.WriteLineAsync("                <Grid container alignItems='center' justifyContent='center' spacing={3.75} sx={{ p: theme => theme.spacing(0.8, 1) }} >");
            //DETALLE GRID
            var columnsArray = table.Columns.Where(f => !f.IsIdentity && f.ColumnName != "AuditId").ToList();
            var gridColumnLayout = Helper.GetColumnLayout(columnsArray.Count);
            var gridColumnSizeArray = gridColumnLayout.Item1.ToArray();
            for (int i = 0; i <= columnsArray.Count - 1; i++)
            {
                await outputFile.WriteLineAsync(string.Concat("                    <Grid item xs={", gridColumnSizeArray[i], "} md={", gridColumnSizeArray[i], "} lg={", gridColumnSizeArray[i], "}>"));
                if (!columnsArray[i].IsForeignKey)
                {
                    await outputFile.WriteLineAsync(string.Concat("                        {item.", Helper.GetCamel(columnsArray[i].ColumnName), "}"));
                }
                else
                {
                    var fkTableInfo = project.Tables.First(f => f.TableName == columnsArray[i].TableTarget);
                    var fkColumnsInfo = fkTableInfo.Columns;
                    var namedColumnInfo = fkColumnsInfo.FirstOrDefault(f => f.ColumnName.ToLower().Contains("nombre") || (f.ColumnName.ToLower().Contains("descripcion") && !f.ColumnName.ToLower().Contains("descripcionid")) || f.ColumnName.ToLower().Contains("codigo"));
                    namedColumnInfo ??= fkColumnsInfo.First(f => f.IsPrimaryKey);
                    await outputFile.WriteLineAsync(string.Concat("                        {item.", Helper.GetCamel(fkTableInfo.TableName), namedColumnInfo.ColumnName, "}"));
                }
                await outputFile.WriteLineAsync($"                    </Grid>");
            }
        
            if (gridColumnLayout.Item2)
            {
                await outputFile.WriteLineAsync(string.Concat("                    <Grid item xs={", gridColumnLayout.Item3, "} md={", gridColumnLayout.Item3, "} lg={", gridColumnLayout.Item3, "}>"));
                await outputFile.WriteLineAsync("                        {item.fechaModificacion}");
                await outputFile.WriteLineAsync($"                    </Grid>");
                await outputFile.WriteLineAsync(string.Concat("                    <Grid item xs={", gridColumnLayout.Item4, "} md={", gridColumnLayout.Item4, "} lg={", gridColumnLayout.Item4, "}>"));
                await outputFile.WriteLineAsync("                        {item.nombreCortoUsuario}");
                await outputFile.WriteLineAsync($"                    </Grid>");
            }
            await outputFile.WriteLineAsync($"                </Grid>");
        
            await outputFile.WriteLineAsync("                <ListItemIcon className={'ListItemIcons'} sx={{position: 'absolute', right: 24, top: 7, transition: 'all 0.2s', opacity: 0 }}>");
            await outputFile.WriteLineAsync("                    <Fab onClick={() => { setOpen(true); setRefItem(item); setCreateMode(false); }} size='small' color={'primary'} sx={{ right: 14 }}><EditIcon /></Fab>");
            string relevantColumnName = pk.ColumnName;
            var nameDescriptionColumn = table.Columns.FirstOrDefault(f => f.ColumnName.ToLower().Contains("nombre") || f.ColumnName.ToLower().Contains("descripcion"));
            if (nameDescriptionColumn != null)
                relevantColumnName = nameDescriptionColumn.ColumnName;
            await outputFile.WriteLineAsync(string.Concat("                    <Fab onClick={() => confirmDelete(item.", Helper.GetCamel(relevantColumnName), ")} size='small' color={'secondary'}><DeleteIcon /></Fab>"));
            await outputFile.WriteLineAsync($"                </ListItemIcon>");
            await outputFile.WriteLineAsync($"            </ListItemButton>");
            await outputFile.WriteLineAsync($"        </Card>");
            await outputFile.WriteLineAsync($"    );");
            await outputFile.WriteLineAsync("};");
            await outputFile.WriteLineAsync($"export default {table.TableName}Item;");
            return;
        }
    }
}
