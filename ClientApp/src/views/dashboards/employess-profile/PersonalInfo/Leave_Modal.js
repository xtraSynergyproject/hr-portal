
import Card from '@mui/material/Card'


import CardContent from '@mui/material/CardContent'





// ** React Imports
import { useState } from 'react'
import { styled } from '@mui/material/styles'

import Paper from '@mui/material/Paper';
import CardMedia from '@mui/material/CardMedia';

// ** MUI Imports
import IconButton from '@mui/material/IconButton'
import Icon from 'src/@core/components/icon'
import Button from '@mui/material/Button'
import Dialog from '@mui/material/Dialog'
import Select from '@mui/material/Select'
import MenuItem from '@mui/material/MenuItem'
import InputLabel from '@mui/material/InputLabel'
import DialogTitle from '@mui/material/DialogTitle'
import FormControl from '@mui/material/FormControl'
import DialogContent from '@mui/material/DialogContent'
import DialogActions from '@mui/material/DialogActions'
import Typography from '@mui/material/Typography'
import Grid from '@mui/material/Grid'

const SelectWithDialog = () => {
  // ** State
  const [open, setOpen] = useState(false)

  const handleClickOpen = () => {
    setOpen(true)
  }


  const Item = styled(Paper)(({ theme }) => ({
    backgroundColor: theme.palette.mode === 'dark' ? '#1A2027' : '#fff',
    ...theme.typography.body2,
    padding: theme.spacing(1),
    textAlign: 'center',
    color: theme.palette.text.secondary,
}));


  const handleClose = () => {
    setOpen(false)
  }

  return (
    <div  >
     <Typography variant='h6' component='span'>
        <Button variant='contained' onClick={handleClickOpen}>
       New Leave  Request 
          
        </Button>
      </Typography>
   
      <Dialog maxWidth='md' fullWidth open={open} onClose={handleClose} >
        <DialogTitle>Create New Leave Request</DialogTitle>
        <IconButton
            aria-label='close'
            onClick={handleClose}
            sx={{ top: 10, right: 10, position: 'absolute', color: 'grey.500' }}
          >
             <Icon icon='mdi:close' />
          </IconButton>
        
        <DialogContent >
        <Card sx={{ position: 'relative', overflow: 'visible', mt: { xs: 0, sm: 14.4, md: 0 } }}>
                <CardContent sx={{ p: theme => theme.spacing(7.25, 7.5, 7.75, 7.5) }}>


                    <Grid container rowSpacing={3}  columnSpacing={20}>
                        <Grid item xs={3}>
                            <Item>

                                <CardMedia
                                    sx={{ height: 140, width: 200 }}
                                    image="https://th.bing.com/th/id/OIP.LciTNhZjw8eFQOLLtG4mawHaGG?pid=ImgDet&rs=1"
                                    title="green iguana"
                                    
         


                                />
                                <Button size="small" >Compassionately Leave</Button>




                            </Item>
                        </Grid>

                        <Grid item xs={3}>
                            <Item>

                                <CardMedia
                                    sx={{ height: 140, width: 200 }}
                                    image="https://th.bing.com/th/id/OIP.BvzRC46VL0qmmwlbIA0WtAHaE7?w=254&h=180&c=7&r=0&o=5&pid=1.7"
                                    title="green iguana"

                                />
                                <Button size="small" >Un Absent</Button>



                            </Item>
                        </Grid>
                        <Grid item xs={3}>
                            <Item>

                                <CardMedia
                                    sx={{ height: 140, width: 200 }}
                                    image="https://th.bing.com/th/id/OIP.Srv_C1WBdLgMg39gyRfxDgHaD4?w=282&h=180&c=7&r=0&o=5&pid=1.7"
                                    title="green iguana"
                                />
                                <Button size="small" >Leave Adjustement</Button>




                            </Item>
                        </Grid>

                        <Grid item xs={3}>
                            <Item>

                                <CardMedia
                                    sx={{ height: 140, width: 200 }}
                                    image="https://www.signaturestaff.com.au/wp-content/uploads/2016/07/leave-cancellation.jpg"
                                    title="green iguana"
                                />
                                <Button size="small" >Leave Cancel</Button>




                            </Item>
                        </Grid>
                        <Grid item xs={3}>
                            <Item>

                                <CardMedia
                                    sx={{ height: 140, width: 200 }}
                                    image="https://th.bing.com/th/id/OIP.7eTnY5DuuU1aNyJcCzyavQHaFj?pid=ImgDet&w=736&h=552&rs=1"
                                    title="green iguana"
                                />
                                <Button size="small" >Marriage Leave</Button>




                            </Item>
                        </Grid>
                        <Grid item xs={3}>
                            <Item>

                                <CardMedia
                                    sx={{ height: 140, width: 200 }}
                                    image="https://th.bing.com/th/id/OIP.0Qtyk58q8SMzAu08QaBkTgHaFb?pid=ImgDet&rs=1"
                                    title="green iguana"
                                />
                                <Button size="small" >Paternity Leave</Button>




                            </Item>
                        </Grid>
                        <Grid item xs={3}>
                            <Item>

                                <CardMedia
                                    sx={{ height: 140, width: 200 }}
                                    image="https://2.bp.blogspot.com/-6CMKoesdwZU/WgCoAzf3VdI/AAAAAAAAMqw/c3u_t2nJctUJCm7rYGG_TzLbBmv44cr7ACLcBGAs/s1600/Maternity2017.jpg"
                                    title="green iguana"
                                />
                                <Button size="small" >Maternity Leave</Button>




                            </Item>
                        </Grid>
                        <Grid item xs={3}>
                            <Item>

                                <CardMedia
                                    sx={{ height: 140, width: 200 }}
                                    image="https://i.pinimg.com/736x/8b/1a/d8/8b1ad84e055b80262af92696b23fb116.jpg"
                                    title="green iguana"
                                />
                                <Button size="small" >Annual Leave Encasement</Button>




                            </Item>
                        </Grid>
                        <Grid item xs={3}>
                            <Item>

                                <CardMedia
                                    sx={{ height: 140, width: 200 }}
                                    image="https://th.bing.com/th/id/OIP.0Y8fmMTjCc2L9YucYXQqKQHaFa?pid=ImgDet&rs=1"
                                    title="green iguana"
                                />
                                <Button size="small" >Sick Leave</Button>




                            </Item>
                        </Grid>
                        <Grid item xs={3}>
                            <Item>

                                <CardMedia
                                    sx={{ height: 140, width: 200 }}
                                    image="https://th.bing.com/th/id/OIP.ZQC7DRRbK-_Ma56FwC9K1gHaE8?pid=ImgDet&rs=1"
                                    title="green iguana"
                                />
                                <Button size="small" >Unpaid Leave</Button>




                            </Item>
                        </Grid>
                        <Grid item xs={3}>
                            <Item>

                                <CardMedia
                                    sx={{ height: 140, width: 200 }}
                                    image="https://blogs.glowscotland.org.uk/nl/public/CardinalNewmanWebsite/uploads/sites/21883/2017/05/study-leave.jpg"
                                    title="green iguana"
                                />
                                <Button size="small" >Examination  Leave</Button>




                            </Item>
                        </Grid>
                        <Grid item xs={3}>
                            <Item>

                                <CardMedia
                                    sx={{ height: 140, width: 200 }}
                                    image="https://th.bing.com/th/id/OIP.4i3AQu4B7vDF9-WSPd1-mQHaFP?pid=ImgDet&rs=1"
                                    title="green iguana"
                                />
                                <Button size="small" > Leave HandOver Service</Button>




                            </Item>
                        </Grid>
                        <Grid item xs={3}>
                            <Item>

                                <CardMedia
                                    sx={{ height: 140, width: 200 }}
                                    image="https://th.bing.com/th/id/OIP.SiC4WD4-GF10pxH-gaqdvgHaHa?pid=ImgDet&rs=1"
                                    title="green iguana"
                                />
                                <Button size="small" >Annual Leave</Button>




                            </Item>
                        </Grid>
                        <Grid item xs={3}>
                            <Item>

                                <CardMedia
                                    sx={{ height: 140, width: 200 }}
                                    image="https://i.ytimg.com/vi/c8NulVkRsHI/maxresdefault.jpg"
                                    title="green iguana"
                                />
                                <Button size="small" >Sample Leave</Button>




                            </Item>
                        </Grid>
                        <Grid item xs={3}>
                            <Item>

                                <CardMedia
                                    sx={{ height: 140, width: 200 }}
                                    image="https://thumbs.dreamstime.com/b/business-man-doing-hard-work-office-22765456.jpg"
                                    title="green iguana"
                                />
                                <Button size="small" >Under Time Leave</Button>




                            </Item>
                        </Grid>



                    </Grid>

                </CardContent>
            </Card>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleClose} variant='outlined'>
            Ok
          </Button>
        </DialogActions>
      </Dialog>
    </div>
  )
}

export default SelectWithDialog














