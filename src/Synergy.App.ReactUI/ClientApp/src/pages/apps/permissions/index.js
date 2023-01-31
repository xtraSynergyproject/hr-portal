// ** React Imports
import { useState, useEffect, useCallback } from 'react'

// ** MUI Imports
import Box from '@mui/material/Box'
import Card from '@mui/material/Card'
import Grid from '@mui/material/Grid'
import Alert from '@mui/material/Alert'
import Button from '@mui/material/Button'
import Dialog from '@mui/material/Dialog'
import { DataGrid } from '@mui/x-data-grid'
import Checkbox from '@mui/material/Checkbox'
import FormGroup from '@mui/material/FormGroup'
import TextField from '@mui/material/TextField'
import IconButton from '@mui/material/IconButton'
import Typography from '@mui/material/Typography'
import AlertTitle from '@mui/material/AlertTitle'
import DialogTitle from '@mui/material/DialogTitle'
import DialogContent from '@mui/material/DialogContent'
import FormControlLabel from '@mui/material/FormControlLabel'

// ** Icon Imports
import Icon from 'src/@core/components/icon'

// ** Store Imports
import { useDispatch, useSelector } from 'react-redux'

// ** Custom Components Imports
import CustomChip from 'src/@core/components/mui/chip'
import PageHeader from 'src/@core/components/page-header'
import TableHeader from 'src/views/apps/permissions/TableHeader'

// ** Actions Imports
import { fetchData } from 'src/store/apps/permissions'

import axios from 'axios'

function createData(ServiceId, UserId, PersonId) {
  return { ServiceId, UserId, PersonId };
}

const colors = {
  support: 'info',
  users: 'success',
  manager: 'warning',
  administrator: 'primary',
  'restricted-user': 'error'
}

const defaultColumns = [
  {
    flex: 0.25,
    field: 'ServiceId',
    minWidth: 240,
    headerName: 'Service Id',
    renderCell: ({ row }) => <Typography>{row.ServiceId} ghvh</Typography>
  },
  {
    flex: 0.25,
    minWidth: 215,
    field: 'UserId',
    headerName: 'UserId',
    renderCell: ({ row }) => <Typography>{row.UserId}</Typography>
  },
  {
    flex: 0.25,
    minWidth: 215,
    field: 'PersonId',
    headerName: 'Personal Id',
    renderCell: ({ row }) => <Typography>{row.PersonId}</Typography>
  
  }


  // {
  //   flex: 0.25,
  //   minWidth: 215,
  //   field: 'createdDate',
  //   headerName: 'Templated Action',
  //   renderCell: ({ row }) => <Typography>{row.createdDate}</Typography>
  // },
  // // {
  //   flex: 0.25,
  //   minWidth: 215,
  //   field: 'createdDate',
  //   headerName: 'Service No',
  //   renderCell: ({ row }) => <Typography>{row.createdDate}</Typography>
  // },
  // {
  //   flex: 0.25,
  //   minWidth: 215,
  //   field: 'createdDate',
  //   headerName: 'Service Subject',
  //   renderCell: ({ row }) => <Typography>{row.createdDate}</Typography>
  // },
  // {
  //   flex: 0.25,
  //   minWidth: 215,
  //   field: 'createdDate',
  //   headerName: 'Service Description',
  //   renderCell: ({ row }) => <Typography>{row.createdDate}</Typography>
  // },
  // {
  //   flex: 0.25,
  //   minWidth: 215,
  //   field: 'createdDate',
  //   headerName: 'Applied Date',
  //   renderCell: ({ row }) => <Typography>{row.createdDate}</Typography>
  // },
  // {
  //   flex: 0.25,
  //   minWidth: 215,
  //   field: 'createdDate',
  //   headerName: 'leave Type',
  //   renderCell: ({ row }) => <Typography>{row.createdDate}</Typography>
  // },
  // {
  //   flex: 0.25,
  //   minWidth: 215,
  //   field: 'createdDate',
  //   headerName: 'Leave Type Code',
  //   renderCell: ({ row }) => <Typography>{row.createdDate}</Typography>
  // },
  // {
  //   flex: 0.25,
  //   minWidth: 215,
  //   field: 'createdDate',
  //   headerName: 'StartDate',
  //   renderCell: ({ row }) => <Typography>{row.createdDate}</Typography>
  // },
  // {
  //   flex: 0.25,
  //   minWidth: 215,
  //   field: 'createdDate',
  //   headerName: 'EndDate',
  //   renderCell: ({ row }) => <Typography>{row.createdDate}</Typography>
  // },
  // {
  //   flex: 0.25,
  //   minWidth: 215,
  //   field: 'createdDate',
  //   headerName: 'LeaveStatusAction',
  //   renderCell: ({ row }) => <Typography>{row.createdDate}</Typography>
  // },
]

const PermissionsTable = () => {
  // ** State
  const [value, setValue] = useState('')
  const [pageSize, setPageSize] = useState(10)
  const [editValue, setEditValue] = useState('')
  const [editDialogOpen, setEditDialogOpen] = useState(false)

  // Api Intregration by using Get method
  const [getdata, setGetdata] = useState([]) 
  const viewData = async () => {
    let response = await axios.get(`https://webapidev.aitalkx.com/chr/leave/ReadLeaveDetailData?userId=45bba746-3309-49b7-9c03-b5793369d73c`)
    console.log(response.data, "response data")
  }
  console.log(getdata, "response")

  useEffect(() => {
    viewData()
  }, [])

  // ** Hooks
  const dispatch = useDispatch()
  const store = useSelector(state => state.permissions)
  useEffect(() => {
    dispatch(
      fetchData({
        q: value
      })
    )
  }, [dispatch, value])

  const handleFilter = useCallback(val => {
    setValue(val)
  }, [])

  const handleEditPermission = name => {
    setEditValue(name)
    setEditDialogOpen(true)
  }
  const handleDialogToggle = () => setEditDialogOpen(!editDialogOpen)

  const onSubmit = e => {
    setEditDialogOpen(false)
    e.preventDefault()
  }

  const columns = [
    ...defaultColumns,
    {
      flex: 0.15,
      minWidth: 115,
      sortable: false,
      field: 'actions',
      headerName: 'Actions',
      renderCell: ({ row }) => (
        <Box sx={{ display: 'flex', alignItems: 'center' }}>
          {/* <IconButton onClick={() => handleEditPermission(row.name)}>
            <Icon icon='mdi:pencil-outline' fontSize={20} />
          </IconButton>
          <IconButton>
            <Icon icon='mdi:delete-outline' fontSize={20} />
          </IconButton> */}
        </Box>
      )
    }
  ]

  return (
    <>
      <Grid container spacing={6}>
        <Grid item xs={12}>
          <PageHeader
            title={<Typography variant='h5'>Permissions List</Typography>}
            subtitle={
              <Typography variant='body2'>
                Each category (Basic, Professional, and Business) includes the four predefined roles shown below.
              </Typography>
            }
          />
        </Grid>
        <Grid item xs={12}>
          <Card>
            <TableHeader value={value} handleFilter={handleFilter} />
            <DataGrid
              autoHeight
              rows={store.data}
              columns={columns}
              pageSize={pageSize}
              disableSelectionOnClick
              rowsPerPageOptions={[10, 25, 50]}
              onPageSizeChange={newPageSize => setPageSize(newPageSize)}
            />
          </Card>
        </Grid>
      </Grid>
      <Dialog maxWidth='sm' fullWidth onClose={handleDialogToggle} open={editDialogOpen}>
        <DialogTitle sx={{ mx: 'auto', textAlign: 'center' }}>
          <Typography variant='h5' component='span' sx={{ mb: 2 }}>
            Edit Permission
          </Typography>
          <Typography variant='body2'>Edit permission as per your requirements.</Typography>
        </DialogTitle>
        <DialogContent sx={{ mx: 'auto' }}>
          <Alert severity='warning' sx={{ maxWidth: '500px' }}>
            <AlertTitle>Warning!</AlertTitle>
            By editing the permission name, you might break the system permissions functionality. Please ensure you're
            absolutely certain before proceeding.
          </Alert>

          <Box component='form' sx={{ mt: 8 }} onSubmit={onSubmit}>
            <FormGroup sx={{ mb: 2, alignItems: 'center', flexDirection: 'row', flexWrap: ['wrap', 'nowrap'] }}>
              <TextField
                fullWidth
                size='small'
                value={editValue}
                label='Permission Name'
                sx={{ mr: [0, 4], mb: [3, 0] }}
                placeholder='Enter Permission Name'
                onChange={e => setEditValue(e.target.value)}
              />

              <Button type='submit' variant='contained'>
                Update
              </Button>
            </FormGroup>
            <FormControlLabel control={<Checkbox />} label='Set as core permission' />
          </Box>
        </DialogContent>
      </Dialog>
    </>
  )
}

export default PermissionsTable
