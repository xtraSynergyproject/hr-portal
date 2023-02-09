// ** React Imports

import Card from '@mui/material/Card'
import Grid from '@mui/material/Grid'
import { DataGrid } from '@mui/x-data-grid'



const CMSGrid = props => {
  // ** Props
  const { row, columns, pageSize } = props
  
  return (

    <Grid container spacing={6}>
      <Grid item xs={12}>
        <Card>
           <DataGrid
            autoHeight            
            rows={row}
            columns={columns}
            pageSize={pageSize}
            disableSelectionOnClick
            rowsPerPageOptions={[10, 25, 50]}
            onPageSizeChange={newPageSize => setPageSize(newPageSize)}
            getRowId={(row) => row.NoteId}
          />
        </Card>
      </Grid>
    </Grid>
  )
}

export default CMSGrid


