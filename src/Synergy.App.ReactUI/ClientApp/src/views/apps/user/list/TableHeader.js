// ** MUI Imports
import Box from '@mui/material/Box'
import Button from '@mui/material/Button'
import TextField from '@mui/material/TextField'
import Model from 'src/views/dashboards/workstructure/Components (1)/Components/Model'

// ** Icon Imports
import Icon from 'src/@core/components/icon'

const TableHeader = props => {
  // ** Props
  const { handleFilter, toggle, value } = props

  return (
    <Box sx={{ p: 5, pb: 3, display: 'flex', flexWrap: 'wrap', alignItems: 'center', justifyContent: 'space-between' }}>
      {/* <Button
        sx={{ mr: 4, mb: 2 }}
        color='secondary'
        variant='outlined'
        startIcon={<Icon icon='mdi:export-variant' fontSize={20} />}
      >
        Export
      </Button> */}
      <Box sx={{ display: 'flex', flexWrap: 'wrap', alignItems: 'center' }}>
        <TextField
          size='small'
          value={value}
          sx={{ mr: 4, mb:1, width:"600px" }}
          placeholder='Search User'
          onChange={e => handleFilter(e.target.value)}
        />

        {/* <Button sx={{ mb: 2 }} onClick={toggle} variant='contained'>
          Create User
        </Button> */}
        <Model/>
      </Box>
    </Box>
  )
}

export default TableHeader
