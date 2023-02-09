// ** MUI Imports
import Box from '@mui/material/Box'
import Card from '@mui/material/Card'
import { DataGrid } from '@mui/x-data-grid'
import CardHeader from '@mui/material/CardHeader'

// ** Data Import
import { rows } from 'src/@fake-db/table/static-data'

const columns = [
  {
    flex: 0.1,
    field: 'id',
    minWidth: 80,
    headerName: 'Doc Type'
  },
  {
    flex: 0.25,
    minWidth: 200,
    field: 'full_name',
    headerName: 'Document Name'
  },
  {
    flex: 0.25,
    minWidth: 230,
    field: 'email',
    headerName: 'Tag Tag Name'
  },
  {
    flex: 0.15,
    minWidth: 130,
    field: 'Actions',
    headerName: 'Date'
  },
//   {
//     flex: 0.15,
//     minWidth: 120,
//     field: 'experience',
//     headerName: 'Last Updated By'
//   },
//   {
//     flex: 0.1,
//     field: 'age',
//     minWidth: 80,
//     headerName: 'Last Updated Date'
//   },
//   {
//     flex: 0.1,
//     field: 'age',
//     minWidth: 80,
//     headerName: 'Created By'
//   },
//   {
//     flex: 0.1,
//     field: 'age',
//     minWidth: 80,
//     headerName: 'Created Date'
//   }
]

const TableBasic = () => {
  return (
    <Card>
      <CardHeader title='Attachment List' />
      <Box sx={{ height: 500 }}>
        <DataGrid columns={columns} rows={rows.slice(0, 10)} />
      </Box>
    </Card>
  )
}

export default TableBasic
