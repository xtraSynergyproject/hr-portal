// ** MUI Imports
import TreeView from '@mui/lab/TreeView'
import TreeItem from '@mui/lab/TreeItem'
import Button from '@mui/material/Button'
import Box from '@mui/material/Box'
import Typography from '@mui/material/Typography'
import Checkbox from '@mui/material/Checkbox'
import FormControlLabel from '@mui/material/FormControlLabel'

// ** Icon Imports
import Icon from 'src/@core/components/icon'

const HelpDiskSidebar = ({ direction }) => {
  const ExpandIcon = direction === 'rtl' ? 'mdi:chevron-left' : 'mdi:chevron-right'

  return (<Box sx={{ m: 3 }}>
    <Box >
      <Button variant='text' color='secondary' size='medium' startIcon={<Icon icon='mdi:home' />}>
        Home
      </Button><br />
      <Button variant='text' color='secondary' size='medium' startIcon={<Icon icon='mdi:filter' />}>
        Pending Till Today
      </Button><br />
      <Button variant='text' color='secondary' size='medium' startIcon={<Icon icon='mdi:edit' />}>
        End in Next 7 days
      </Button><br />
      <Button variant='text' color='secondary' size='medium' startIcon={<Icon icon='mdi:delete' />}>
        Delete
      </Button>
    </Box>



    <TreeView
      sx={{ minHeight: 240 }}
      defaultExpandIcon={<Icon icon={ExpandIcon} />}
      defaultCollapseIcon={<Icon icon='mdi:chevron-down' />}
    >

      <TreeItem nodeId='1' label='Modules' sx={{ my: 5 }}>
        <Typography>Filtered By Status: All</Typography>
        <Typography><Button color='secondary'>Clear Filter</Button></Typography>
        <TreeItem nodeId='15' label=''>
          <FormControlLabel label='All' control={<Checkbox name='basic-unchecked' />} /><br />
          <FormControlLabel label='Case Management System' control={<Checkbox name='basic-unchecked' />} /><br />
        </TreeItem>
      </TreeItem>


      <TreeItem nodeId='5' label='Status Filers' sx={{ my: 5 }}>
        <Typography>Filtered By Status: All</Typography>
        <Typography><Button color='secondary'>Clear Filter</Button></Typography>
        <TreeItem nodeId='7' label=''>
          <FormControlLabel label='All' control={<Checkbox name='basic-unchecked' />} /><br />
          <FormControlLabel label='Draft' control={<Checkbox name='basic-unchecked' />} /><br />
          <FormControlLabel label='Cancel' control={<Checkbox name='basic-unchecked' />} /><br />
          <FormControlLabel label='Complete' control={<Checkbox name='basic-unchecked' />} /><br />
          <FormControlLabel label='Overdue' control={<Checkbox name='basic-unchecked' />} /><br />
          <FormControlLabel label='In Progress' control={<Checkbox name='basic-unchecked' />} /><br />
        </TreeItem>
      </TreeItem>


      <TreeItem nodeId='11' label='Person Filters' sx={{ my: 5 }}>
        <Typography>Filtered By Status: All</Typography>
        <Typography><Button color='secondary'>Clear Filter</Button></Typography>
        <TreeItem nodeId='16' label=''>
          <FormControlLabel label='All' control={<Checkbox name='basic-unchecked' />} /><br />
          <FormControlLabel label='Requested by me' control={<Checkbox name='basic-unchecked' />} /><br />
          <FormControlLabel label='Shared with me/Team' control={<Checkbox name='basic-unchecked' />} /><br />
        </TreeItem>
      </TreeItem>

    </TreeView>
  </Box>
  )
}

export default HelpDiskSidebar







