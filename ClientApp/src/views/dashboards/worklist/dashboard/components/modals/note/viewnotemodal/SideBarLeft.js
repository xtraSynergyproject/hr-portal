import TreeView from '@mui/lab/TreeView'
import TreeItem from '@mui/lab/TreeItem'
import Button from '@mui/material/Button'
import Box from '@mui/material/Box'
import { Fragment, useState, useEffect } from 'react'
import Typography from '@mui/material/Typography'
import Checkbox from '@mui/material/Checkbox'
import FormControlLabel from '@mui/material/FormControlLabel'
// ** Icon Imports
import Icon from 'src/@core/components/icon'

const SidebarLeft = ({ direction }) => {
  const ExpandIcon = direction === 'rtl' ? 'mdi:chevron-left' : 'mdi:chevron-right'

  return (
    <Box sx={{ m: 3, width: '250px' }}>
      <Box>
        <Button variant='text' color='primary' size='medium' startIcon={<Icon icon='mdi:inbox' />}>
          Inbox
        </Button>
        <br />
        <Button variant='text' color='secondary' size='medium' startIcon={<Icon icon='mdi:calendar-alert' />}>
           Today
        </Button>
        <br />
        <Button variant='text' color='secondary' size='medium' startIcon={<Icon icon='mdi:calendar-search' />}>
           Next 7 days
        </Button>
        <br />
        
      </Box>
      <TreeView
        sx={{ minHeight: 240 }}
        defaultExpandIcon={<Icon icon={ExpandIcon} />}
        defaultCollapseIcon={<Icon icon='mdi:chevron-down' />}
      >
        <TreeItem nodeId='1' backgroundcolor="private" label='Modules' variant='conatined' sx={{ my: 5 }}>
          <Typography sx={{ margin: '12px' }}>Filtered By Status: All</Typography>
          <Typography>
            <Button color='secondary'>Clear Filter</Button>
          </Typography>
          <TreeItem nodeId='15' label=''>
            <FormControlLabel label='All' control={<Checkbox name='basic-unchecked' />} />
            <br />
            <FormControlLabel label='Case Management System' control={<Checkbox name='basic-unchecked' />} />
            <br />
          </TreeItem>
        </TreeItem>

        <TreeItem nodeId='5' label='Status Filers' sx={{ my: 5 }}>
          <Typography sx={{ margin: '12px' }}>Filtered By Status: All</Typography>
          <Typography>
            <Button color='secondary'>Clear Filter</Button>
          </Typography>
          <TreeItem nodeId='7' label=''>
            <FormControlLabel label='All' control={<Checkbox name='basic-unchecked' />} />
            <br />
            <FormControlLabel label='Draft' control={<Checkbox name='basic-unchecked' />} />
            <br />
            <FormControlLabel label='Cancel' control={<Checkbox name='basic-unchecked' />} />
            <br />
            <FormControlLabel label='Complete' control={<Checkbox name='basic-unchecked' />} />
            <br />
            <FormControlLabel label='Overdue' control={<Checkbox name='basic-unchecked' />} />
            <br />
            <FormControlLabel label='In Progress' control={<Checkbox name='basic-unchecked' />} />
            <br />
          </TreeItem>
        </TreeItem>

        <TreeItem nodeId='11' label='Person Filters' sx={{ my: 5 }}>
          <Typography sx={{ margin: '12px' }}>Filtered By Status: All</Typography>
          <Typography>
            <Button color='primary'>Clear Filter</Button>
          </Typography>
          <TreeItem nodeId='16' label=''>
            <FormControlLabel label='All' control={<Checkbox name='basic-unchecked' />} />
            <br />
            <FormControlLabel label='Requested by me' control={<Checkbox name='basic-unchecked' />} />
            <br />
            <FormControlLabel label='Shared with me/Team' control={<Checkbox name='basic-unchecked' />} />
            <br />
          </TreeItem>
        </TreeItem>
      </TreeView>
    </Box>
  )
}
export default SidebarLeft