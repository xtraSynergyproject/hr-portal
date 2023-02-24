// ** MUI Imports
import Box from '@mui/material/Box'
import TreeView from '@mui/lab/TreeView'
import TreeItem from '@mui/lab/TreeItem'
import Checkbox from '@mui/material/Checkbox'
import Icon from 'src/@core/components/icon'
import Card from '@mui/material/Card'
import Button from '@mui/material/Button'
import Typography from '@mui/material/Typography'
import CardContent from '@mui/material/CardContent'
import FormControlLabel from '@mui/material/FormControlLabel'
import CardActions from '@mui/material/CardActions'

const RequestSidebarLeft = ({ direction }) => {
  const ExpandIcon = direction === 'rtl' ? 'mdi:chevron-left' : 'mdi:chevron-right'
  return (
    <Card>
      <CardContent>
        <Typography variant='body2' sx={{ mb: 3.25 }}>
          <Box>
            <Button variant='text' color='primary' size='medium' startIcon={<Icon icon='mdi:home' />}>
              Home
            </Button>
            <br />
            <Button variant='text' color='secondary' size='medium' startIcon={<Icon icon='mdi:filter' />}>
              Pending Till Today
            </Button>
            <br />
            <Button variant='text' color='secondary' size='medium' startIcon={<Icon icon='mdi:edit' />}>
              End in Next 7 days
            </Button>
            <br />
            <Button variant='text' color='secondary' size='medium' startIcon={<Icon icon='mdi:delete' />}>
              Delete
            </Button>
          </Box>
        </Typography>
        <Typography variant='body2'>
          <TreeView
            sx={{ minHeight: 240 }}
            defaultExpandIcon={<Icon icon={ExpandIcon} />}
            defaultCollapseIcon={<Icon icon='mdi:chevron-down' />}
          >
            <TreeItem nodeId='1' label='Modules' color='secondary' sx={{ my: 5, border: '1px dashed grey' }}>
              <Typography sx={{ margin: '12px' }}>
                <b>
                  Filtered <br />
                  By
                  <br /> Module
                </b>
                : All
              </Typography>
              <Typography>
                <Button variant='text' color='primary'>
                  Clear
                </Button>
              </Typography>

              <Typography>
                <Button variant='text' color='primary'>
                  Filter
                </Button>
              </Typography>

              <TreeItem nodeId='15' label='' component='span' sx={{ border: '1px dashed grey' }}>
                <FormControlLabel label='All' control={<Checkbox name='basic-unchecked' />} />
                <br />
                <FormControlLabel label='Case Management System' control={<Checkbox name='basic-unchecked' />} />
                <br />
              </TreeItem>
            </TreeItem>

            <TreeItem nodeId='5' label='Status Filters' sx={{ my: 5, border: '1px dashed grey' }}>
              <Typography sx={{ margin: '12px' }}>
                <b>
                  Filtered <br />
                  By
                  <br /> Status
                </b>
                : All
              </Typography>
              <Typography>
                <Button variant='text' color='primary'>
                  Clear
                </Button>
              </Typography>

              <Typography>
                <Button variant='text' color='primary'>
                  Filter
                </Button>
              </Typography>

              <TreeItem nodeId='7' label='' component='span' sx={{ border: '1px dashed grey' }}>
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

            <TreeItem nodeId='11' label='Person Filters' sx={{ my: 5, border: '1px dashed grey' }}>
              <Typography sx={{ margin: '12px' }}>
                <b>
                  Filtered <br />
                  By
                  <br /> Role
                </b>
                : All
              </Typography>
              <Typography>
                <Button variant='text' color='primary'>
                  Clear
                </Button>
              </Typography>

              <Typography>
                <Button variant='text' color='primary'>
                  Filter
                </Button>
              </Typography>
              <TreeItem nodeId='16' label='' component='span' sx={{ border: '1px dashed grey' }}>
                <FormControlLabel label='All' control={<Checkbox name='basic-unchecked' />} />
                <br />
                <FormControlLabel label='Requested by me' control={<Checkbox name='basic-unchecked' />} />
                <br />
                <FormControlLabel label='Shared with me/Team' control={<Checkbox name='basic-unchecked' />} />
                <br />
              </TreeItem>
            </TreeItem>
          </TreeView>
        </Typography>
      </CardContent>
      <CardActions className='card-action-dense'></CardActions>
    </Card>
  )
}

export default RequestSidebarLeft
