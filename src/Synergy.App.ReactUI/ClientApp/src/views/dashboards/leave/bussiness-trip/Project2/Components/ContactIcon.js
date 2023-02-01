// ** MUI Imports
import Grid from '@mui/material/Grid'
import Card from '@mui/material/Card'
import Link from '@mui/material/Link'
import Button from '@mui/material/Button'
import Tooltip from '@mui/material/Tooltip'
import Typography from '@mui/material/Typography'
import CardContent from '@mui/material/CardContent'


// ** Custom Components Imports
import Icon from 'src/@core/components/icon'
import PageHeader from 'src/@core/components/page-header'
import ModeToggler from 'src/@core/layouts/components/shared-components/ModeToggler'

const icons = [

 'mdi:account-check',

]

const Icons = () => {
  const renderIconGrids = () => {
    return icons.map((icon, index) => {
      return (
        <Grid item key={index} sx={{margin:"22px"}}>
          <Tooltip arrow title={icon} placement='top'>
            <Card>
              <Button  variant="contained" sx={{ display: 'flex',Color:'#fff'}}>
                <Icon icon={icon} />
              
                
                
              </Button>
            </Card>
          </Tooltip>
        </Grid>
      )
    })
  }

  return (
    <Grid container spacing={6}>
    
      
         <Grid container spacing={4} >
           {renderIconGrids()}
         </Grid>
     
    </Grid>
  )
}

export default Icons
