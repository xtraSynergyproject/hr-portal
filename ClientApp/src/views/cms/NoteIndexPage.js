// ** React Imports
import { useState, useEffect, useCallback } from 'react'


// ** Next Imports
import Link from 'next/link'

// ** MUI Imports
import Box from '@mui/material/Box'
import Card from '@mui/material/Card'
import Menu from '@mui/material/Menu'
import Grid from '@mui/material/Grid'
import Divider from '@mui/material/Divider'
import { DataGrid } from '@mui/x-data-grid'
import { styled } from '@mui/material/styles'
import MenuItem from '@mui/material/MenuItem'
import IconButton from '@mui/material/IconButton'
import Typography from '@mui/material/Typography'
import Button from '@mui/material/Button'
import CardHeader from '@mui/material/CardHeader'
import InputLabel from '@mui/material/InputLabel'
import FormControl from '@mui/material/FormControl'
import CardContent from '@mui/material/CardContent'
import Select from '@mui/material/Select'
import Icon from 'src/@core/components/icon'

// ** Store Imports
import { useDispatch, useSelector } from 'react-redux'


// ** Third Party Components
import axios from 'axios'

import CMSDataGrid from './components/CMSDataGrid'





const RowOptions = ({ id }) => {
  // ** Hooks
  const dispatch = useDispatch()

  // ** State
  const [anchorEl, setAnchorEl] = useState(null)
  const rowOptionsOpen = Boolean(anchorEl)

  const handleRowOptionsClick = event => {
    setAnchorEl(event.currentTarget)
  }

  const handleRowOptionsClose = () => {
    setAnchorEl(null)
  }

  const handleDelete = () => {
    dispatch(deleteUser(id))
    handleRowOptionsClose()
  }

  return (
    <>
      <IconButton size='small' onClick={handleRowOptionsClick}>
        <Icon icon='mdi:dots-vertical' />
      </IconButton>
      <Menu
        keepMounted
        anchorEl={anchorEl}
        open={rowOptionsOpen}
        onClose={handleRowOptionsClose}
        anchorOrigin={{
          vertical: 'bottom',
          horizontal: 'right'
        }}
        transformOrigin={{
          vertical: 'top',
          horizontal: 'right'
        }}
        PaperProps={{ style: { minWidth: '8rem' } }}
      >
        <MenuItem
          component={Link}
          sx={{ '& svg': { mr: 2 } }}
          onClick={handleRowOptionsClose}
          href='/apps/user/view/overview/'
        >
          <Icon icon='mdi:eye-outline' fontSize={20} />
          View
        </MenuItem>
        <MenuItem onClick={handleRowOptionsClose} sx={{ '& svg': { mr: 2 } }}>
          <Icon icon='mdi:pencil-outline' fontSize={20} />
          Edit
        </MenuItem>
        <MenuItem onClick={handleDelete} sx={{ '& svg': { mr: 2 } }}>
          <Icon icon='mdi:delete-outline' fontSize={20} />
          Delete
        </MenuItem>
      </Menu>
    </>
  )
}



const UserList = ({ apiData }) => {

  //Index Page
  const [popupCallbackMethod, setPopupCallbackMethod] = useState([])
  const [pageId, setPageId] = useState([])
  const [pageType, setPageType] = useState([])
  const [templateCodes, setTemplateCodes] = useState([])
  const [indexPageTemplateId, setIndexPageTemplateId] = useState([])
  const [hideSummaryTabs, setHideSummaryTabs] = useState([])
  const [pageTitle, setPageTitle] = useState([])
  const [isVersioningButtonVisible, setIsVersioningButtonVisible] = useState([])
  const [isViewButtonVisible, setIsViewButtonVisible] = useState([])
  const [isDeleteButtonVisible, setIsDeleteButtonVisible] = useState([])
  const [templateId, setTemplateId] = useState([])
  const [deleteConfirmationMessage, setDeleteConfirmationMessage] = useState([])


  const [pageSize, setPageSize] = useState(10)
  // Api intergration by using get method
  const [getdata, setGetdata] = useState([])
  const [columns, setColumns] = useState([])
  const viewData = async () => {

    let colresponse = await axios.get(`https://webapidev.aitalkx.com/cms/NtsNote/GetNoteIndexColumn?pageId=dab0d921-cabc-4008-840b-4d1097dc47c3`)

    const model = colresponse.data;
    console.log(model, "model");

    setPopupCallbackMethod(model.Page.PopupCallbackMethod);
    setPageId(model.Page.Id);
    setPageType(model.Page.PageType);
    setTemplateCodes(model.TemplateCodes);
    setIndexPageTemplateId(model.Id);
    setHideSummaryTabs(model.HideSummaryTabs);
    setPageTitle(model.Page.Title);
    setIsVersioningButtonVisible(model.IsVersioningButtonVisible);
    setIsViewButtonVisible(model.IsViewButtonVisible);
    setIsDeleteButtonVisible(model.IsDeleteButtonVisible);
    setTemplateId(model.TemplateId);
    setDeleteConfirmationMessage(model.DeleteConfirmationMessage);
    setGetdata(model.TableRowData);
    //Manipulate data
    // setColumndata(colresponse.data)
    const columndata = model.SelectedTableRows;
    var ColData = [];
    for (var i = 0; i < columndata.length; i++) {
      console.log(columndata[i].ColumnName, "ColumnName");
      console.log(columndata[i].ColumnHeaderName, "ColumnHeaderName");
      ColData.push({ 'field': columndata[i].ColumnName, 'headerName': columndata[i].ColumnHeaderName, 'minWidth': 110, 'flex': 0.1 });
    }
    if (columndata.length > 0) {
      ColData.push({
        'field': 'actions', 'headerName': 'Actions', 'minWidth': 70, 'flex': 0.1, 'renderCell': ({ row }) => <RowOptions id={row.id} />
      });
    }
    setColumns(ColData);

  }


  useEffect(() => {
    viewData()
  }, [])



  return (
    <Grid container spacing={6}>

      <Grid item xs={12}>
        <Card>
          <CardHeader title={pageTitle} />
          <CMSDataGrid row={getdata} columns={columns} pageSize={pageSize} />

        </Card>
      </Grid>

    </Grid>
  )
}



export default UserList








