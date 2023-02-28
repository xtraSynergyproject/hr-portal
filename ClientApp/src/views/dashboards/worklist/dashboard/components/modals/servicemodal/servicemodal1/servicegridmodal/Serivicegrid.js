// ** MUI Imports
import {Grid,Box} from '@mui/material'
import Card from '@mui/material/Card'
import Table from '@mui/material/Table'
import TableRow from '@mui/material/TableRow'
import TableBody from '@mui/material/TableBody'
import TableCell from '@mui/material/TableCell'
import TableHead from '@mui/material/TableHead'
import CardHeader from '@mui/material/CardHeader'
import CardContent from '@mui/material/CardContent'
import TableContainer from '@mui/material/TableContainer'
import { useEffect, useCallback, useState } from 'react'


// ** Icon Imports
import Icon from 'src/@core/components/icon'

// Component Imports
import CustomAvatar from 'src/@core/components/mui/avatar'
import { Divider } from '@mui/material'
import Typography from '@mui/material/Typography'
// import AccountCircleIcon from '@mui/icons-material/AccountCircle';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import axios from 'axios'
import SearchBar from '../../../note/viewnotemodal/SearchBar'




const CardActions = () => {
    // Api Intregration by using Get method
  const [getdata, setGetdata] = useState([])
  const viewData = async () => {
  

    try {
      let response = await axios.get(
        'https://webapidev.aitalkx.com/cms/service/ReadServiceData?userId=45bba746-3309-49b7-9c03-b5793369d73c&portalName=HR'
      )
      console.log(response);
      setGetdata(response.data)
    }catch(error) {
      console.log(error);
    }
    
  }
  console.log(getdata, 'response')

  useEffect(() => {
    viewData()
  }, [])

  return (
    <>
    <Grid container spacing={6}>
      <Grid item xs={12}>
        <Card sx={{width:'50rem'}}>
            <SearchBar/>
            <Divider/>

          <CardContent>
            <TableContainer>
              <Table sx={{ minWidth:'40rem' }} aria-label='Card Actions'>
              {getdata.map((data) => (

                <TableBody>
                  <TableRow>
                    <TableCell component='th' scope='row'>
                    <CustomAvatar variant='circle'>S</CustomAvatar>

                    </TableCell>
                    <TableCell align='center'>
                    <Box>
                        <Box sx={{display:'flex',ml:4}}>
                        <Typography sx={{color:'blue',fontsize:15}}>
                          {data.ServiceSubject} 
                     </Typography><span>{data.ServiceNo}</span>
                        </Box>
                     
                      <Box>
                     <Typography sx={{fontSize:15}}>Administrator to Administrator<span><EditIcon />
                      <DeleteIcon /></span></Typography>
                       </Box>
                    </Box>
                    </TableCell>
                    <TableCell>
                        <Box>
                     <Typography sx={{color:'green'}}>{data.ServiceStatusName}</Typography>
                      <Typography sx={{}}>{data.DueDateDisplay}</Typography>
                     </Box>
                     </TableCell>
                  </TableRow>
                 
                </TableBody>
                              ))}

              </Table>
            </TableContainer>
          </CardContent>
        </Card>
      </Grid>

      
    </Grid>
    </>
  )
}

export default CardActions
